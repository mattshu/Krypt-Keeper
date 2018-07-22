using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using KryptKeeper.Properties;

namespace KryptKeeper
{
    public partial class MainWindow : Form
    {
        /* 
         * TODO *
         * - Fix column resize problem when resizing past border
        */

        private Status status;

        private bool _settingsNeedConfirmed = true;
        private bool _settingsNotViewed = false;

        private List<FileData> _fileList = new List<FileData>();

        public MainWindow()
        {
            InitializeComponent();
        }

        private void loadFileListColumnWidths()
        {
            var widths = Settings.Default.fileListColumnWidths;
            for (int i = 0; i < FileListGridView.ColumnCount; i++)
                FileListGridView.Columns[i].Width = int.Parse(widths[i]);
        }

        private void mainWindow_Shown(object sender, EventArgs e)
        {
            loadSettings();
            status = new Status(txtStatus);
        }

        private void loadSettings()
        {
            var settings = Settings.Default;
            var algorithms = settings.algorithms;
            foreach (var a in algorithms)
            {
                cbxEncryptAlgorithms.Items.Add(a);
                cbxDecryptAlgorithms.Items.Add(a);
            }
            setDefaultCbxIndexes();
            if (!settings.rememberSettings) return;
            cbxEncryptAlgorithms.SelectedIndex = settings.encryptionAlgorithm;
            chkMaskInformation.Checked = settings.encryptionMaskInformation;
            cbxMaskInformation.SelectedIndex = settings.encryptionMaskInfoType;
            chkRemoveAfterEncrypt.Checked = settings.encryptionRemoveAfterEncrypt;
            cbxEncryptionKeyType.SelectedIndex = settings.encryptionKeyType;
            txtEncryptionKey.Text = settings.encryptionKey;
            chkRememberSettings.Checked = true;
            chkConfirmOnExit.Checked = settings.confirmOnExit;
            if (settings.useEncryptionSettings)
                copyEncryptionSettings();
        }

        private void saveSettings()
        {
            var settings = Settings.Default;
            settings.encryptionAlgorithm = cbxEncryptAlgorithms.SelectedIndex;
            settings.encryptionKey = txtEncryptionKey.Text;
            settings.encryptionKeyType = cbxEncryptionKeyType.SelectedIndex;
            settings.encryptionMaskInformation = chkMaskInformation.Checked;
            settings.encryptionMaskInfoType = cbxMaskInformation.SelectedIndex;
            settings.encryptionRemoveAfterEncrypt = chkRemoveAfterEncrypt.Checked;
            settings.useEncryptionSettings = chkUseEncryptSettings.Checked;
            settings.rememberSettings = chkRememberSettings.Checked;
            settings.confirmOnExit = chkConfirmOnExit.Checked;
            settings.decryptionAlgorithm = cbxDecryptAlgorithms.SelectedIndex;
            settings.decryptionKey = txtDecryptionKey.Text;
            settings.decryptionKeyType = cbxDecryptionKeyType.SelectedIndex;
            settings.Save();
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

        private bool settingsAreDefault()
        {
            if (_settingsNotViewed) return true;
            return !txtEncryptionKey.Modified || !txtDecryptionKey.Modified;
        }

        private static void resetSettings()
        {
            var settings = Settings.Default;
            settings.encryptionAlgorithm = settings.decryptionAlgorithm = -1;
            settings.encryptionKeyType = settings.decryptionKeyType = -1;
            settings.encryptionKey = settings.decryptionKey = "";
            settings.encryptionMaskInfoType = -1;
            settings.encryptionMaskInformation = false;
            settings.encryptionRemoveAfterEncrypt = true;
            settings.rememberSettings = false;
            settings.useEncryptionSettings = true;
            settings.confirmOnExit = true;
            settings.Save();
        }

        private void setDefaultCbxIndexes()
        {
            cbxEncryptAlgorithms.SelectedIndex = 0;
            cbxMaskInformation.SelectedIndex = 0;
            cbxEncryptionKeyType.SelectedIndex = 0;
            cbxDecryptAlgorithms.SelectedIndex = 0;
            cbxDecryptionKeyType.SelectedIndex = 0;
        }

        private void resetFileList()
        {
            FileListGridView.DataSource = null;
        }

        private void refreshFileListGridView()
        {
            FileListGridView.DataSource = null;
            FileListGridView.DataSource = _fileList;
        }

        private void enableControls(bool state)
        {
            btnEncrypt.Enabled = btnDecrypt.Enabled = state;
        }

        private void btnAddFiles_Click(object sender, EventArgs e)
        {
            buildFileList();
        }

        private void buildFileList()
        {
            var openFileDialog = new OpenFileDialog { Multiselect = true };
            var openResult = openFileDialog.ShowDialog();
            if (openResult != DialogResult.OK) return;
            FileListGridView.Columns.Clear();
            _fileList = openFileDialog.FileNames.Select(path => new FileData(path)).ToList();
            FileListGridView.DataSource = _fileList;
            loadFileListColumnWidths();
            enableControls(FileListGridView.RowCount > 0);
            tabMain.SelectedIndex = 0;
        }

        private void btnRemoveFiles_Click(object sender, EventArgs e)
        {
            removeSelectedFiles();
        }

        private void removeSelectedFiles()
        {
            var selectedCount = FileListGridView.SelectedRows.Count;
            if (selectedCount <= 0) return;
            for (int i = FileListGridView.RowCount - 1; i >= 0; i--)
                if (FileListGridView.Rows[i].Selected)
                    _fileList.RemoveAt(i);
            refreshFileListGridView();
            FileListGridView.ClearSelection();
        }

        private void tabMain_TabIndexChanged(object sender, EventArgs e)
        {
            if (tabMain.TabIndex == 1)
                _settingsNotViewed = false;
        }

        private void btnEncryptAll_Click(object sender, EventArgs e)
        {
            if (confirmSettings())
                processAllFiles(CipherOptions.Encrypt);
        }

        private void btnEncryptSelected_Click(object sender, EventArgs e)
        {
            if (confirmSettings())
                processSelectedFiles(CipherOptions.Encrypt);
        }

        private void btnDecryptAll_Click(object sender, EventArgs e)
        {
            if (confirmSettings())
                processAllFiles(CipherOptions.Decrypt);
        }

        private void btnDecryptSelected_Click(object sender, EventArgs e)
        {
            if (confirmSettings())
                processSelectedFiles(CipherOptions.Decrypt);
        }

        private void processAllFiles(int cipherMode)
        {
            if (!validateSettings(cipherMode))
                return;
            focusStatusTab();
            var paths = getPathsFromFileList();
            var options = generateOptions(cipherMode);
            if (cipherMode == CipherOptions.Encrypt)
                Cipher.EncryptFiles(paths, options);
            else
                Cipher.DecryptFiles(paths, options);
            resetFileList();
        }

        private bool validateSettings(int mode)
        {
            if (_fileList.Count <= 0) return false;
            if (mode == CipherOptions.Encrypt && string.IsNullOrWhiteSpace(txtEncryptionKey.Text) ||
                mode == CipherOptions.Decrypt && string.IsNullOrWhiteSpace(txtDecryptionKey.Text)) return false;
            return true;
        }

        private void focusStatusTab()
        {
            tabMain.SelectedIndex = 2;
            tabMain.Refresh();
        }

        private void processSelectedFiles(int cipherMode)
        {
            if (!validateSettings(cipherMode))
                return;
            if (FileListGridView.SelectedRows.Count <= 0) return;
            focusStatusTab();
            var options = generateOptions(cipherMode);
            var paths = getPathsFromSelection();
            if (cipherMode == CipherOptions.Encrypt)
                Cipher.EncryptFiles(paths, options);
            else
                Cipher.DecryptFiles(paths, options);
            resetFileList();
        }

        private CipherOptions generateOptions(int cipherOption)
        {
            ComboBox algorithm;
            ComboBox keyType;
            TextBox keytxt;
            var key = new byte[0];

            if (cipherOption == CipherOptions.Encrypt)
            {
                algorithm = cbxEncryptAlgorithms;
                keyType = cbxEncryptionKeyType;
                keytxt = txtEncryptionKey;
            }
            else if (cipherOption == CipherOptions.Decrypt)
            {
                algorithm = cbxDecryptAlgorithms;
                keyType = cbxDecryptionKeyType;
                keytxt = txtDecryptionKey;
            }
            else
            {
                throw new Exception("Invalid cipher option: " + cipherOption);
            }
            if (keyType.SelectedIndex == 0)
            {
                key = Encoding.Default.GetBytes(keytxt.Text);
            }
            else if (keyType.SelectedIndex == 1)
            {
                if (!File.Exists(keytxt.Text))
                    throw new FileNotFoundException(keytxt.Text);
                key = File.ReadAllBytes(keytxt.Text);
            }
            var maskInfoIndex = cbxMaskInformation.SelectedIndex;
            var options = new CipherOptions
            {
                Mode = (CipherAlgorithm)algorithm.SelectedIndex,
                Key = key,
                MaskFileName = chkMaskInformation.Checked && (maskInfoIndex == 0 || maskInfoIndex == 2),
                MaskFileTimes = chkMaskInformation.Checked && (maskInfoIndex == 1 || maskInfoIndex == 2),
                RemoveOriginal = chkRemoveAfterEncrypt.Checked
            };
            options.GenerateIV();
            return options;
        }

        private string[] getPathsFromFileList()
        {
            if (_fileList.Count <= 0) throw new Exception("Cannot generate paths from empty file list!");
            return _fileList.Select(file => file.GetFilePath()).ToArray();
        }

        private string[] getPathsFromSelection()
        {
            var selectedCount = FileListGridView.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedCount <= 0) throw new Exception("Cannot generate paths from empty selection!");
            var paths = new List<string>();
            var selectedRows = FileListGridView.SelectedRows;
            for (int i = 0; i < selectedCount; i++)
            {
                var file = _fileList[selectedRows[i].Index];
                paths.Add(file.GetFilePath());
            }
            return paths.ToArray();
        }

        private void cbxEncryptAlgorithms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (chkUseEncryptSettings.Checked)
                cbxDecryptAlgorithms.SelectedIndex = cbxEncryptAlgorithms.SelectedIndex;
        }

        private void chkMaskInformation_CheckedChanged(object sender, EventArgs e)
        {
            cbxMaskInformation.Enabled = chkMaskInformation.Checked;
        }

        private void chkUseEncryptSettings_CheckedChanged(object sender, EventArgs e)
        {
            if (chkUseEncryptSettings.Checked) copyEncryptionSettings();
            cbxDecryptAlgorithms.Enabled = !chkUseEncryptSettings.Checked;
            cbxDecryptionKeyType.Enabled = !chkUseEncryptSettings.Checked;
            txtDecryptionKey.ReadOnly = chkUseEncryptSettings.Checked;
        }

        private void copyEncryptionSettings()
        {
            cbxDecryptAlgorithms.SelectedIndex = cbxEncryptAlgorithms.SelectedIndex;
            cbxDecryptionKeyType.SelectedIndex = cbxEncryptionKeyType.SelectedIndex;
            txtDecryptionKey.Text = txtEncryptionKey.Text;
            btnBrowseDecrypt.Enabled = btnBrowseEncrypt.Enabled;
        }

        private void cbxEncryptionKeyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateFormBasedOnKeyType(cbxEncryptionKeyType, btnBrowseEncrypt, txtEncryptionKey);
            if (chkUseEncryptSettings.Checked)
                cbxDecryptionKeyType.SelectedIndex = cbxEncryptionKeyType.SelectedIndex;
        }

        private static void updateFormBasedOnKeyType(ListControl cbxKeyType, Control btnBrowse, TextBox txtKey)
        {
            btnBrowse.Enabled = cbxKeyType.SelectedIndex == 1; // Key file
            if (cbxKeyType.SelectedIndex == 1) txtKey.Text = "";
        }

        private void txtEncryptionKey_TextChanged(object sender, EventArgs e)
        {
            if (chkUseEncryptSettings.Checked)
                txtDecryptionKey.Text = txtEncryptionKey.Text;
        }

        private void cbxDecryptionKeyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateFormBasedOnKeyType(cbxDecryptionKeyType, btnBrowseDecrypt, txtDecryptionKey);
        }

        private void btnBrowseEncrypt_Click(object sender, EventArgs e)
        {
            txtEncryptionKey.Text = browseForKeyFile();
        }

        private void btnBrowseDecrypt_Click(object sender, EventArgs e)
        {
            txtDecryptionKey.Text = browseForKeyFile();
        }

        private static string browseForKeyFile()
        {
            var openFile = new OpenFileDialog();
            if (openFile.ShowDialog() != DialogResult.OK || !openFile.CheckFileExists) return "";
            return openFile.FileName;
        }

        private void fileListGridView_SelectionChanged(object sender, EventArgs e)
        {
            btnRemoveFiles.Enabled = btnEncryptSelected.Enabled =
                btnDecryptSelected.Enabled = FileListGridView.SelectedRows.Count > 0;
        }

        private void fileListGridView_DataSourceChanged(object sender, EventArgs e)
        {
            enableControls(FileListGridView.RowCount > 0);
        }

        private void txtStatus_TextChanged(object sender, EventArgs e)
        {
            btnExport.Enabled = txtStatus.Text.Length > 0;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtStatus.Clear();
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            exportStatusLog();
        }

        private void exportStatusLog()
        {
            var saveFileDialog = new SaveFileDialog {DefaultExt = "log", Filter = @"Log files(*.log)|*.*"};
            var dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK || string.IsNullOrWhiteSpace(saveFileDialog.FileName)) return;
            using (var fStream = saveFileDialog.OpenFile())
            {
                var logHeader = generateLogHeader();
                fStream.Write(logHeader, 0, logHeader.Length);
                fStream.Write(Encoding.Default.GetBytes(txtStatus.Text), 0, txtStatus.TextLength);
            }
            status.WriteLine("Exported log to " + saveFileDialog.FileName);
        }

        private static byte[] generateLogHeader()
        {
            var timestamp = DateTime.Now;
            string header = "KryptKeeper Status Log -- Generated on " + timestamp; // TODO INSERT VERSION INFORMATION
            return Encoding.Default.GetBytes(header);
        }

        private void saveFileListColumnWidths()
        {
            var enumer = FileListGridView.Columns.GetEnumerator();
            var widths = new StringCollection();
            while (enumer.MoveNext())
            {
                var header = (DataGridViewTextBoxColumn) enumer.Current;
                if (header == null) break;
                widths.Add(header.Width.ToString());
            }
            if (widths.Count <= 0) return;
            Settings.Default.fileListColumnWidths = widths;
            Settings.Default.Save();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (!confirmExit())
            {
                e.Cancel = true;
                return;
            }
            saveFileListColumnWidths();
            if (chkRememberSettings.Checked)
            {
                saveSettings();
                return;
            }
            if (settingsAreDefault())
            {
                resetSettings();
                return;
            }
            var result = MessageBox.Show(@"Do you want to save your settings?", @"Save Settings?",
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
            if (result != DialogResult.Yes)
            {
                if (result == DialogResult.Cancel)
                    e.Cancel = true;
                else
                    resetSettings();
            }
            else
                saveSettings();
        }

        private bool confirmExit()
        {
            if (chkConfirmOnExit.Checked)
                return MessageBox.Show(@"Are you sure you want to exit?", @"Exit KryptKeeper",
                       MessageBoxButtons.OKCancel,
                       MessageBoxIcon.Question) == DialogResult.OK;
            return true;
        }
    }
}