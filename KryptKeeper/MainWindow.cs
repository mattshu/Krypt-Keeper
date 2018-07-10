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

        private readonly Status _status;

        private bool _settingsNeedConfirmed = true;
        private bool _settingsNotViewed = false;

        private List<FileData> _fileList = new List<FileData>();

        public MainWindow()
        {
            InitializeComponent();
            _status = new Status(TxtStatus);
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
            Debug.WriteLine(Helper.GetRandomNumericString(12));
            Debug.WriteLine(Helper.GetRandomNumericString(6));
            Debug.WriteLine(Helper.GetRandomNumericString(4));
        }

        private void loadSettings()
        {
            var settings = Settings.Default;
            var algorithms = settings.algorithms;
            foreach (var a in algorithms)
            {
                CbxEncryptAlgorithms.Items.Add(a);
                CbxDecryptAlgorithms.Items.Add(a);
            }
            setDefaultCbxIndexes();
            if (!settings.rememberSettings) return;
            CbxEncryptAlgorithms.SelectedIndex = settings.encryptionAlgorithm;
            ChkMaskInformation.Checked = settings.encryptionMaskInformation;
            CbxMaskInformation.SelectedIndex = settings.encryptionMaskInfoType;
            ChkRemoveAfterEncrypt.Checked = settings.encryptionRemoveAfterEncrypt;
            CbxEncryptionKeyType.SelectedIndex = settings.encryptionKeyType;
            TxtEncryptionKey.Text = settings.encryptionKey;
            ChkRememberSettings.Checked = true;
            if (settings.useEncryptionSettings)
                copyEncryptionSettings();
        }

        private void setDefaultCbxIndexes()
        {
            CbxEncryptAlgorithms.SelectedIndex = 0;
            CbxMaskInformation.SelectedIndex = 0;
            CbxEncryptionKeyType.SelectedIndex = 0;
            CbxDecryptAlgorithms.SelectedIndex = 0;
            CbxDecryptionKeyType.SelectedIndex = 0;
        }

        private void btnAddFiles_Click(object sender, EventArgs e)
        {
            addFilesToList();
        }

        private void addFilesToList()
        {
            var openFileDialog = new OpenFileDialog {Multiselect = true};
            var openResult = openFileDialog.ShowDialog();
            if (openResult != DialogResult.OK) return;
            FileListGridView.Columns.Clear();
            _fileList = openFileDialog.FileNames.Select(path => new FileData(path)).ToList();
            FileListGridView.DataSource = _fileList;
            loadFileListColumnWidths();
            enableControls(FileListGridView.RowCount > 0);
            TabMain.SelectedIndex = 0;
        }

        private void enableControls(bool state)
        {
            BtnEncrypt.Enabled = BtnDecrypt.Enabled = state;
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

        private void refreshFileListGridView()
        {
            FileListGridView.DataSource = null;
            FileListGridView.DataSource = _fileList;
        }

        private bool settingsAreDefault()
        {
            if (_settingsNotViewed) return true;
            return !TxtEncryptionKey.Modified || !TxtDecryptionKey.Modified;
        }

        private void tabMain_TabIndexChanged(object sender, EventArgs e)
        {
            if (TabMain.TabIndex == 1)
                _settingsNotViewed = false;
        }

        private void btnEncryptAll_Click(object sender, EventArgs e)
        {
            if (confirmSettings())
                encryptAllFiles();
        }

        private void encryptAllFiles()
        {
            var options = generateOptions(CipherOptions.Encrypt);
            if (_fileList.Count <= 0) return;
            foreach (var file in _fileList)
            {
                var fullPath = Path.Combine(file.Path, file.Name);
                _status.WritePending("Encrypting " + fullPath);
                Cipher.Encrypt(fullPath, options);
                _status.PendingComplete();
            }
        }

        private CipherOptions generateOptions(int cipherOption)
        {
            ComboBox algorithm;
            ComboBox keyType;
            TextBox keyTxt;
            var key = new byte[0];

            if (cipherOption == CipherOptions.Encrypt)
            {
                algorithm = CbxEncryptAlgorithms;
                keyType = CbxEncryptionKeyType;
                keyTxt = TxtEncryptionKey;
            }
            else if (cipherOption == CipherOptions.Decrypt)
            {
                algorithm = CbxDecryptAlgorithms;
                keyType = CbxDecryptionKeyType;
                keyTxt = TxtDecryptionKey;
            }
            else
            {
                throw new Exception("Invalid cipher option: " + cipherOption);
            }
            if (keyType.SelectedIndex == 0)
            {
                key = Encoding.Default.GetBytes(keyTxt.Text);
            }
            else if (keyType.SelectedIndex == 1)
            {
                if (!File.Exists(keyTxt.Text))
                    throw new FileNotFoundException(keyTxt.Text);
                key = File.ReadAllBytes(keyTxt.Text);
            }
            var options = new CipherOptions
            {
                Mode = (CipherAlgorithm) algorithm.SelectedIndex,
                Key = key,
                MaskFileName = ChkMaskInformation.Checked && CbxMaskInformation.SelectedIndex == 0 ||
                               CbxMaskInformation.SelectedIndex == 2,
                MaskFileTimes = ChkMaskInformation.Checked && CbxMaskInformation.SelectedIndex == 1 ||
                                CbxMaskInformation.SelectedIndex == 2,
                RemoveOriginal = ChkRemoveAfterEncrypt.Checked
            };
            return options;
        }

        private void btnEncryptSelected_Click(object sender, EventArgs e)
        {
            if (confirmSettings())
                encryptSelectedFiles();
        }

        private void encryptSelectedFiles()
        {
            var options = generateOptions(CipherOptions.Encrypt);
            if (_fileList.Count <= 0) return;
            var selectedCount = FileListGridView.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedCount <= 0) return;
            for (int i = 0; i < selectedCount; i++)
            {
                var file = _fileList[FileListGridView.SelectedRows[i].Index];
                var fullPath = Path.Combine(file.Path, file.Name);
                _status.WritePending("Encrypting " + fullPath);
                Cipher.Encrypt(fullPath, options);
                _status.PendingComplete();
            }
            resetFileList();
        }

        private void resetFileList()
        {
            //FileListGridView.DataSource = null;
            foreach (var file in _fileList.ToList())
                if (!File.Exists(Path.Combine(file.Path, file.Name)))
                    _fileList.Remove(file);
            FileListGridView.DataSource = _fileList;
        }

        private bool confirmSettings()
        {
            if (!_settingsNeedConfirmed) return true;
            var confirmSettingsDialog = new ConfirmSettingsDialog();
            var confirmSettingsResult = confirmSettingsDialog.ShowDialog();
            _settingsNeedConfirmed = confirmSettingsDialog.ShowAgain;
            if (confirmSettingsResult != DialogResult.No) return true;
            TabMain.SelectTab(1);
            return false;
        }

        private void btnDecryptAll_Click(object sender, EventArgs e)
        {
            if (confirmSettings())
                decryptAllFiles();
        }

        private void decryptAllFiles()
        {
            var options = generateOptions(CipherOptions.Decrypt);
            if (_fileList.Count <= 0) return;
            foreach (var file in _fileList)
            {
                var fullPath = Path.Combine(file.Path, file.Name);
                _status.WritePending("Decrypting " + fullPath);
                Cipher.Encrypt(fullPath, options);
                _status.PendingComplete();
            }
            resetFileList();
        }

        private void btnDecryptSelected_Click(object sender, EventArgs e)
        {
            if (confirmSettings())
                decryptSelectedFiles();
        }

        private void decryptSelectedFiles()
        {
            var options = generateOptions(CipherOptions.Decrypt);
            if (_fileList.Count <= 0) return;
            var selectedCount = FileListGridView.Rows.GetRowCount(DataGridViewElementStates.Selected);
            if (selectedCount <= 0) return;
            for (int i = 0; i < selectedCount; i++)
            {
                var file = _fileList[FileListGridView.SelectedRows[i].Index];
                var fullPath = Path.Combine(file.Path, file.Name);
                _status.WritePending("Decrypting " + fullPath);
                Cipher.Encrypt(fullPath, options);
                _status.PendingComplete();
            }
            resetFileList();
        }

        private void cbxEncryptAlgorithms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ChkUseEncryptSettings.Checked)
                CbxDecryptAlgorithms.SelectedIndex = CbxEncryptAlgorithms.SelectedIndex;
        }

        private void chkMaskInformation_CheckedChanged(object sender, EventArgs e)
        {
            CbxMaskInformation.Enabled = ChkMaskInformation.Checked;
        }

        private void chkUseEncryptSettings_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkUseEncryptSettings.Checked) copyEncryptionSettings();
            CbxDecryptAlgorithms.Enabled = !ChkUseEncryptSettings.Checked;
            CbxDecryptionKeyType.Enabled = !ChkUseEncryptSettings.Checked;
            TxtDecryptionKey.ReadOnly = ChkUseEncryptSettings.Checked;
        }

        private void copyEncryptionSettings()
        {
            CbxDecryptAlgorithms.SelectedIndex = CbxEncryptAlgorithms.SelectedIndex;
            CbxDecryptionKeyType.SelectedIndex = CbxEncryptionKeyType.SelectedIndex;
            TxtDecryptionKey.Text = TxtEncryptionKey.Text;
            BtnBrowseDecrypt.Enabled = BtnBrowseEncrypt.Enabled;
        }

        private void cbxEncryptionKeyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateFormBasedOnKeyType(CbxEncryptionKeyType, BtnBrowseEncrypt, TxtEncryptionKey);
            if (ChkUseEncryptSettings.Checked)
                CbxDecryptionKeyType.SelectedIndex = CbxEncryptionKeyType.SelectedIndex;
        }

        private void txtEncryptionKey_TextChanged(object sender, EventArgs e)
        {
            if (ChkUseEncryptSettings.Checked)
                TxtDecryptionKey.Text = TxtEncryptionKey.Text;
        }

        private void cbxDecryptionKeyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            updateFormBasedOnKeyType(CbxDecryptionKeyType, BtnBrowseDecrypt, TxtDecryptionKey);
        }

        private static void updateFormBasedOnKeyType(ListControl cbxKeyType, Control btnBrowse, TextBox txtKey)
        {
            btnBrowse.Enabled = cbxKeyType.SelectedIndex == 1; // Key file
            if (cbxKeyType.SelectedIndex == 1) txtKey.Text = "";
        }

        private void btnBrowseEncrypt_Click(object sender, EventArgs e)
        {
            TxtEncryptionKey.Text = browseForKeyFile();
        }

        private void btnBrowseDecrypt_Click(object sender, EventArgs e)
        {
            TxtDecryptionKey.Text = browseForKeyFile();
        }

        private void fileList_SelectionChanged(object sender, EventArgs e)
        {
            BtnRemoveFiles.Enabled = BtnEncryptSelected.Enabled =
                BtnDecryptSelected.Enabled = FileListGridView.SelectedRows.Count > 0;
        }

        private static string browseForKeyFile()
        {
            var openFile = new OpenFileDialog();
            if (openFile.ShowDialog() != DialogResult.OK || !openFile.CheckFileExists) return "";
            return openFile.FileName;
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void txtStatus_TextChanged(object sender, EventArgs e)
        {
            BtnExport.Enabled = TxtStatus.Text.Length > 0;
        }

        private void mainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            saveFileListColumnWidths();
            if (ChkRememberSettings.Checked)
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
            if (result == DialogResult.Yes)
                saveSettings();
            else if (result == DialogResult.Cancel)
                e.Cancel = true;
            else
                resetSettings();
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

        private void saveSettings()
        {
            var settings = Settings.Default;
            settings.encryptionAlgorithm = CbxEncryptAlgorithms.SelectedIndex;
            settings.encryptionKey = TxtEncryptionKey.Text;
            settings.encryptionKeyType = CbxEncryptionKeyType.SelectedIndex;
            settings.encryptionMaskInformation = ChkMaskInformation.Checked;
            settings.encryptionMaskInfoType = CbxMaskInformation.SelectedIndex;
            settings.useEncryptionSettings = ChkUseEncryptSettings.Checked;
            settings.rememberSettings = ChkRememberSettings.Checked;
            settings.decryptionAlgorithm = CbxDecryptAlgorithms.SelectedIndex;
            settings.decryptionKey = TxtDecryptionKey.Text;
            settings.decryptionKeyType = CbxDecryptionKeyType.SelectedIndex;
            settings.Save();
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
            settings.Save();
        }

        private void fileListGridView_DataSourceChanged(object sender, EventArgs e)
        {
            enableControls(FileListGridView.RowCount > 0);
        }
    }
}