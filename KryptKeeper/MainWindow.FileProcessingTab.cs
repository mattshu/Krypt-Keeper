using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using MetroFramework;

namespace KryptKeeper
{
    public partial class MainWindow
    {
        private const int BCRYPT_SALT_ROUNDS = 5;
        private readonly int[] DEFAULT_COLUMN_WIDTHS = {274, 86, 315};

        #region File Processing Tab Form Events
        private void btnSelectFiles_Click(object sender, EventArgs e)
        {
            buildFileList();
        }

        private void datagridFileList_SelectionChanged(object sender, EventArgs e)
        {
            btnRemoveSelectedFiles.Enabled = datagridFileList.SelectedRows.Count > 0;
        }

        private void datagridFileList_DataSourceChanged(object sender, EventArgs e)
        {
            if (datagridFileList.DataSource == null) return;
            setDefaultColumnWidths();
            enableProcessButtons(datagridFileList.RowCount > 0);
        }

        private void btnRemoveSelectedFiles_Click(object sender, EventArgs e)
        {
            removeSelectedFiles();
        }

        private void cbxFileListOrderBySelectedIndexChanged(object sender, EventArgs e)
        {
            sortFileList();
        }

        private void chkProcessOrderDesc_CheckedChanged(object sender, EventArgs e)
        {
            sortFileList();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            processFiles(Cipher.Mode.Encrypt);
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            processFiles(Cipher.Mode.Decrypt);
        }
        #endregion

        private void buildFileList()
        {
            if (!validateKeySettings()) return;
            var newFileList = new FileList(Utils.GetFilesFromDialog(), datagridFileList);
            if (newFileList.Count <= 0) return;
            _fileList = newFileList;
            refreshFileList();
            focusTab(MainTabs.Files);
        }

        private void refreshFileList()
        {
            sortFileList();
            enableProcessButtons(datagridFileList.RowCount > 0);
            updateFileListStats();
        }

        private void updateFileListStats()
        {
            lblJobInformation.Text = $@"{Utils.GetTotalBytes(_fileList).BytesToSizeString()} ({_fileList.Count} files) to be processed.";
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
            else if (radPlaintextKey.Checked && !Utils.CheckSecurePassword(txtCipherKey.Text))
                errorMsg =
                    $"Password must be 8 or more characters, including at least one number, one upper/lowercase, and one symbol: ({string.Join(",", Cipher.ALLOWED_PLAINTEXT_KEY_SYMBOLS)})";
            if (errorMsg.Length <= 0) return true;
            MetroMessageBox.Show(this, errorMsg, "Invalid Settings", MessageBoxButtons.OK, MessageBoxIcon.Error);
            promptForKeyFile();
            return false;
        }

        private void promptForKeyFile()
        {
            focusTab(MainTabs.Options);
            if (radKeyFile.Checked)
                txtCipherKey.Text = Utils.BrowseFiles(@"Select a key file:", false);
        }

        private void enableProcessButtons(bool state)
        {
            btnEncrypt.Enabled = btnDecrypt.Enabled = state;
        }

        private void setDefaultColumnWidths()
        {
            if (datagridFileList.ColumnCount <= 0) return;
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
            {
                if (!datagridFileList.Rows[i].Selected) continue;
                _fileList.RemoveAt(i);
            }
            _fileList.UpdateDataSource();
            updateFileListStats();
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
                _status.Error("Unable to find file: " + ex.Message);
            }
            catch (FileLoadException ex)
            {
                _status.Error("File is either empty or too large (>=4GB): " + ex.Message);
            }
        }

        private void disableButtonsDuringOperation(bool disable = true)
        {
            btnSelectFiles.Enabled = btnAddFiles.Enabled = btnRemoveSelectedFiles.Enabled =
                btnEncrypt.Enabled = btnDecrypt.Enabled = btnSelectFilesFromStatusTab.Enabled = 
                cbxFileListOrderBy.Enabled = chkFileListOrderDesc.Enabled = !disable;
            btnCancelOperation.Enabled = disable;
        }

        private bool validateAllSettings()
        {
            if (!validateKeySettings()) return false;
            if (_fileList.Count > 0) return true;
            MetroMessageBox.Show(this, "There are no files to work.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            focusTab(MainTabs.Files);
            return false;
        }

        private CipherOptions generateOptions(Cipher.Mode cipherMode)
        {
            var key = getKey();
            var salt = Utils.GetBytes(BCrypt.GenerateSalt(BCRYPT_SALT_ROUNDS));
            var options = new CipherOptions
            {
                Mode = cipherMode,
                Files = _fileList,
                Key = key,
                Salt = salt,
                MaskFileName = chkMaskFileInformation.Checked && chkMaskFileName.Checked,
                MaskFileDate = chkMaskFileInformation.Checked && chkMaskFileDate.Checked,
                RemoveOriginalEncryption = chkRemoveAfterEncryption.Checked,
                RemoveOriginalDecryption = chkRemoveAfterDecryption.Checked
            };
            return options;
        }

        private byte[] getKey()
        {
            byte[] key;
            if (radPlaintextKey.Checked)
                key = Utils.GetBytes(txtCipherKey.Text);
            else if (radKeyFile.Checked && File.Exists(txtCipherKey.Text))
            {
                if (new FileInfo(txtCipherKey.Text).Length < Cipher.MAX_FILE_LENGTH)
                    key = File.ReadAllBytes(txtCipherKey.Text);
                else
                    throw new FileLoadException(txtCipherKey.Text);
            }
            else
                throw new FileNotFoundException(txtCipherKey.Text);
            return key;
        }

        private void sortFileList()
        {
            if (_fileList.Count <= 0) return;
            sortFiles((Cipher.ProcessOrder)cbxFileListOrderBy.SelectedIndex, chkFileListOrderDesc.Checked);
        }

        private void sortFiles(Cipher.ProcessOrder processOrder, bool descending = false)
        {
            var fileListComparer = new FileListComparer(processOrder, descending);
            _fileList.Sort(fileListComparer);
            _fileList.UpdateDataSource();
        }

    }
}
