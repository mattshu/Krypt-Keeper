using System;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using KryptKeeper.Properties;
using MetroFramework;

namespace KryptKeeper
{
    public partial class MainWindow
    {
        #region File Processing Tab Form Events
        private void btnAddFiles_Click(object sender, EventArgs e)
        {
            buildFileList();
        }

        private void datagridFileList_SelectionChanged(object sender, EventArgs e)
        {
            btnRemoveSelectedFiles.Enabled = datagridFileList.SelectedRows.Count > 0;
        }

        private void datagridFileList_DataSourceChanged(object sender, EventArgs e)
        {
            enableProcessButtons(datagridFileList.RowCount > 0);
        }

        private void btnRemoveSelectedFiles_Click(object sender, EventArgs e)
        {
            removeSelectedFiles();
        }

        private void chkProcessInOrder_CheckedChanged(object sender, EventArgs e)
        {
            cbxProcessOrderBy.Enabled = chkProcessOrderDesc.Enabled = chkProcessInOrder.Checked;
        }

        private void cbxProcessOrderBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (_fileList.Count <= 0) return;
            sortFiles((ProcessOrder)cbxProcessOrderBy.SelectedIndex, chkProcessOrderDesc.Checked);
        }

        private void chkProcessOrderDesc_CheckedChanged(object sender, EventArgs e)
        {
            if (_fileList.Count <= 0) return;
            sortFiles((ProcessOrder)cbxProcessOrderBy.SelectedIndex, chkProcessOrderDesc.Checked);
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (confirmSettings())
                processFiles(CipherMode.Encrypt);
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (confirmSettings())
                processFiles(CipherMode.Decrypt);
        }
        #endregion

        private void buildFileList()
        {
            if (!validateKeySettings())
            {
                focusTab(MainTabs.Options);
                if (radKeyFile.Checked)
                    txtCipherKey.Text = Helper.BrowseFiles(@"Select a key file", false);
                return;
            }
            var openFileDialog = new OpenFileDialog { Title = @"Select the files to be processed", Multiselect = true };
            var openResult = openFileDialog.ShowDialog();
            if (openResult != DialogResult.OK) return;
            datagridFileList.Columns.Clear();
            _fileList = new FileList(openFileDialog.FileNames.Select(path => new FileData(path)).ToList());
            refreshFileListGridView();
            enableProcessButtons(datagridFileList.RowCount > 0);
            lblJobInformation.Text = $@"{Helper.BytesToString(Helper.CalculateTotalFilePayload(_fileList))} ({_fileList.Count} files) to be processed.";
            focusTab(MainTabs.Files);
        }

        private bool validateKeySettings()
        {
            string errorMsg = "";
            if (radKeyFile.Checked)
            {
                if (!File.Exists(txtCipherKey.Text))
                    errorMsg = "Key file does not exist!";
                else if (new FileInfo(txtCipherKey.Text).Length <= 0)
                    errorMsg = "Key file is empty!";
            }
            else if (!validatePlaintextKey())
                errorMsg = "Passkey does not meet complex standards (8+ characters)"; // TODO include numbers, capitals, and symbols
            if (errorMsg.Length <= 0) return true;
            MetroMessageBox.Show(this, errorMsg, "Invalid Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private bool validatePlaintextKey()
        {
            if (!radPlaintextKey.Checked) return false;
            string plaintext = txtCipherKey.Text;
            return plaintext.Length >= MINIMUM_PLAINTEXT_KEY_LENGTH;
            // TODO more strength testing (a-z,A-Z,0-9,@$%&!etc)
        }

        private void enableProcessButtons(bool state)
        {
            btnEncrypt.Enabled = btnDecrypt.Enabled = state;
        }

        private void removeSelectedFiles()
        {
            var selectedCount = datagridFileList.SelectedRows.Count;
            if (selectedCount <= 0) return;
            for (int i = datagridFileList.RowCount - 1; i >= 0; i--)
                if (datagridFileList.Rows[i].Selected)
                    _fileList.RemoveAt(i);
            datagridFileList.ClearSelection();
            refreshFileListGridView(retainOrder: true);
        }

        private void refreshFileListGridView(bool retainOrder = false)
        {
            datagridFileList.DataSource = null;
            datagridFileList.DataSource = _fileList.GetList();
            if (retainOrder) return;
            arrangeFileListColumns();
            loadFileListColumnWidths();
        }


        private void arrangeFileListColumns()
        {
            var columnOrderFromSettings = Settings.Default.fileListColumnOrder;
            for (int i = 0; i < columnOrderFromSettings.Count; i++)
                datagridFileList.Columns[i].DisplayIndex = int.Parse(columnOrderFromSettings[i]);
        }

        private void loadFileListColumnWidths()
        {
            var widths = Settings.Default.fileListColumnWidths;
            for (int i = 0; i < datagridFileList.ColumnCount; i++)
                datagridFileList.Columns[i].Width = int.Parse(widths[i]);
        }

        private void sortFiles(ProcessOrder processOrder, bool descending = false)
        {
            if (_fileList.Count <= 0) return;
            var fileListComparer = new FileListComparer(processOrder, descending);
            _fileList.Sort(fileListComparer);
            refreshFileListGridView(retainOrder: true);
        }

        private void processFiles(CipherMode cipherMode)
        {
            try
            {
                if (!validateAllSettings()) return;
                focusTab(MainTabs.Status);
                btnCancelOperation.Enabled = true;
                btnSelectFilesFromStatusTab.Enabled = false;
                var options = generateOptions(cipherMode);
                Cipher.ProcessFiles(options);
                resetFileList();
            }
            catch (FileNotFoundException ex)
            {
                _status.WriteLine("* Error: Unable to find keyfile: " + ex.Message);
            }
            catch (FileLoadException ex)
            {
                _status.WriteLine("* Error: Keyfile is either empty or too large (>=4GB): " + ex.Message);
            }
        }

        private bool validateAllSettings()
        {
            if (!validateKeySettings()) return false;
            if (_fileList.Count > 0) return true;
            Helper.ShowErrorBox("There are no files to work.");
            focusTab(MainTabs.Files);
            return false;
        }

        private CipherOptions generateOptions(CipherMode cipherMode)
        {
            byte[] key;
            if (radPlaintextKey.Checked)
                key = Helper.GetBytes(txtCipherKey.Text);
            else if (radKeyFile.Checked && File.Exists(txtCipherKey.Text))
            {
                if (new FileInfo(txtCipherKey.Text).Length < Cipher.MAX_FILE_LENGTH)
                    key = File.ReadAllBytes(txtCipherKey.Text);
                else
                    throw new FileLoadException(txtCipherKey.Text);
            }
            else
                throw new FileNotFoundException(txtCipherKey.Text);
            var options = new CipherOptions
            {
                Mode = cipherMode,
                Files = _fileList,
                Key = key,
                Salt = Helper.GetBytes(BCrypt.GenerateSalt()),
                MaskFileName = chkMaskFileInformation.Checked && chkMaskFileName.Checked,
                MaskFileDate = chkMaskFileInformation.Checked && chkMaskFileDate.Checked,
                RemoveOriginalEncryption = chkRemoveAfterEncryption.Checked,
                RemoveOriginalDecryption = chkRemoveAfterDecryption.Checked
            };
            return options;
        }

        private void resetFileList()
        {
            datagridFileList.DataSource = null;
            lblJobInformation.Text = "";
        }

        private bool confirmSettings()
        {
            if (!_settingsNeedConfirmed) return true;
            var confirmSettingsDialog = new ConfirmSettingsDialog();
            var confirmSettingsResult = confirmSettingsDialog.ShowDialog();
            _settingsNeedConfirmed = confirmSettingsDialog.ShowAgain;
            if (confirmSettingsResult != DialogResult.No) return true;
            tabMain.SelectTab(1);
            return false;
        }

    }
}
