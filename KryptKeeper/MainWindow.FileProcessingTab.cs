using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using MetroFramework;

namespace KryptKeeper
{
    public partial class MainWindow
    {
        private readonly int[] DEFAULT_COLUMN_WIDTHS = {235, 100, 54, 284};

        #region File Processing Tab Form Events
        private void btnAddFiles_Click(object sender, EventArgs e)
        {
            _fileList.Clear();
            buildFileList();
        }

        private void datagridFileList_SelectionChanged(object sender, EventArgs e)
        {
            btnRemoveSelectedFiles.Enabled = datagridFileList.SelectedRows.Count > 0;
        }

        private void datagridFileList_DataSourceChanged(object sender, EventArgs e)
        {
            if (datagridFileList.DataSource != null) setDefaultColumnWidths();
            enableProcessButtons(datagridFileList.RowCount > 0);
        }

        private void btnRemoveSelectedFiles_Click(object sender, EventArgs e)
        {
            removeSelectedFiles();
        }

        private void chkProcessInOrder_CheckedChanged(object sender, EventArgs e)
        {
            cbxProcessOrderBy.Enabled = chkProcessOrderDesc.Enabled = chkProcessInOrder.Checked;
            sortFileList();
        }

        private void cbxProcessOrderBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            sortFileList();
        }

        private void chkProcessOrderDesc_CheckedChanged(object sender, EventArgs e)
        {
            sortFileList();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (confirmSettings())
                processFiles(Cipher.Mode.Encrypt);
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (confirmSettings())
                processFiles(Cipher.Mode.Decrypt);
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
            _fileList.Clear();
            _fileList = new FileList(openFileDialog.FileNames.Select(path => new FileData(path)).ToList());
            datagridFileList.DataSource = _fileList.GetList();
            if (chkProcessInOrder.Checked)
                sortFileList();
            enableProcessButtons(datagridFileList.RowCount > 0);
            lblJobInformation.Text = $@"{Helper.BytesToString(Helper.CalculateTotalFilePayload( _fileList))} ({_fileList.Count} files) to be processed.";
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
            else if (radPlaintextKey.Checked && !Helper.CheckSecurePassword(txtCipherKey.Text))
                errorMsg =
                    $"Password must be 8 or more characters, including at least one number, one upper/lowercase, and one symbol: ({string.Join(",", Cipher.ALLOWED_PLAINTEXT_KEY_SYMBOLS)})";
            if (errorMsg.Length <= 0) return true;
            MetroMessageBox.Show(this, errorMsg, "Invalid Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return false;
        }

        private void enableProcessButtons(bool state)
        {
            btnEncrypt.Enabled = btnDecrypt.Enabled = state;
        }

        private void setDefaultColumnWidths()
        {
            for (int i = 0; i < DEFAULT_COLUMN_WIDTHS.Length; i++)
            {
                datagridFileList.Columns[i].Width = DEFAULT_COLUMN_WIDTHS[i];
            }
        }

        private void removeSelectedFiles()
        {
            var selectedCount = datagridFileList.SelectedRows.Count;
            if (selectedCount <= 0) return;
            for (int i = datagridFileList.RowCount - 1; i >= 0; i--)
                if (datagridFileList.Rows[i].Selected)
                    _fileList.RemoveAt(i);
            datagridFileList.ClearSelection();
        }

        private void processFiles(Cipher.Mode cipherMode)
        {
            try
            {
                if (!validateAllSettings()) return;
                focusTab(MainTabs.Status);
                disableButtonsDuringOperation();
                var options = generateOptions(cipherMode);
                Cipher.ProcessFiles(options);
            }
            catch (FileNotFoundException ex)
            {
                _status.WriteLine("* Error: Unable to find file: " + ex.Message);
            }
            catch (FileLoadException ex)
            {
                _status.WriteLine("* Error: File is either empty or too large (>=4GB): " + ex.Message);
            }
        }


        private void disableButtonsDuringOperation(bool disable = true)
        {
            btnSelectFiles.Enabled = btnAddFiles.Enabled = btnRemoveSelectedFiles.Enabled =
                btnEncrypt.Enabled = btnDecrypt.Enabled = btnSelectFilesFromStatusTab.Enabled = 
                chkProcessInOrder.Enabled = cbxProcessOrderBy.Enabled = chkProcessOrderDesc.Enabled =
                !disable;
            btnCancelOperation.Enabled = disable;
        }

        private bool validateAllSettings()
        {
            if (!validateKeySettings()) return false;
            if (_fileList.Count > 0) return true;
            Helper.ShowErrorBox("There are no files to work.");
            focusTab(MainTabs.Files);
            return false;
        }

        private CipherOptions generateOptions(Cipher.Mode cipherMode)
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
                Salt = Helper.GetBytes(BCrypt.GenerateSalt(10)),
                MaskFileName = chkMaskFileInformation.Checked && chkMaskFileName.Checked,
                MaskFileDate = chkMaskFileInformation.Checked && chkMaskFileDate.Checked,
                RemoveOriginalEncryption = chkRemoveAfterEncryption.Checked,
                RemoveOriginalDecryption = chkRemoveAfterDecryption.Checked
            };
            return options;
        }

        private void sortFileList()
        {
            if (_fileList.Count <= 0) return;
            sortFiles((Cipher.ProcessOrder)cbxProcessOrderBy.SelectedIndex, chkProcessOrderDesc.Checked);
        }

        private void sortFiles(Cipher.ProcessOrder processOrder, bool descending = false)
        {
            datagridFileList.DataSource = null;
            var fileListComparer = new FileListComparer(processOrder, descending);
            _fileList.Sort(fileListComparer);
            datagridFileList.DataSource = _fileList.GetList();
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
