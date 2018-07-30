using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
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
            status = new Status(txtStatus, progressBar);
            Cipher.SetBackgroundWorker(backgroundWorker);
        }

        private void loadSettings()
        {
            var settings = Settings.Default;
            setDefaultCbxIndexes();
            if (!settings.rememberSettings) return;
            chkMaskInformation.Checked = settings.encryptionMaskInformation;
            cbxMaskInformation.SelectedIndex = settings.encryptionMaskInfoType;
            chkRemoveAfterEncrypt.Checked = settings.encryptionRemoveAfterEncrypt;
            cbxCipherKeyType.SelectedIndex = settings.cipherKeyType;
            chkRememberSettings.Checked = true;
            chkConfirmOnExit.Checked = settings.confirmOnExit;
            chkSaveKey.Checked = settings.saveKey;
            if (settings.saveKey)
                txtCipherKey.Text = settings.cipherKey;
        }

        private void saveSettings()
        {
            var settings = Settings.Default;
            settings.saveKey = chkSaveKey.Checked;
            settings.cipherKey = chkSaveKey.Checked ? txtCipherKey.Text : "";
            settings.cipherKeyType = cbxCipherKeyType.SelectedIndex;
            settings.encryptionMaskInformation = chkMaskInformation.Checked;
            settings.encryptionMaskInfoType = cbxMaskInformation.SelectedIndex;
            settings.encryptionRemoveAfterEncrypt = chkRemoveAfterEncrypt.Checked;
            settings.rememberSettings = chkRememberSettings.Checked;
            settings.confirmOnExit = chkConfirmOnExit.Checked;
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
            return !txtCipherKey.Modified;
        }

        private static void resetSettings()
        {
            var settings = Settings.Default;
            settings.cipherKeyType = settings.cipherKeyType = -1;
            settings.cipherKey = "";
            settings.encryptionMaskInfoType = -1;
            settings.encryptionMaskInformation = false;
            settings.encryptionRemoveAfterEncrypt = true;
            settings.rememberSettings = false;
            settings.saveKey = false;
            settings.confirmOnExit = true;
            settings.Save();
        }

        private void setDefaultCbxIndexes()
        {
            cbxMaskInformation.SelectedIndex = 0;
            cbxCipherKeyType.SelectedIndex = 0;
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

        private void btnRemoveSelectedFiles_Click(object sender, EventArgs e)
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

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (confirmSettings())
                processFiles(CipherOptions.ENCRYPT);
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (confirmSettings())
                processFiles(CipherOptions.DECRYPT);
        }

        private void processFiles(int cipherMode)
        {
            if (!validateSettings())
                return;
            focusStatusTab();
            var options = generateOptions(cipherMode);
            Cipher.ProcessFiles(options);
            resetFileList();
        }

        private bool validateSettings()
        {
            if (_fileList.Count <= 0) return false;
            return !string.IsNullOrWhiteSpace(txtCipherKey.Text);
        }

        private void focusStatusTab()
        {
            tabMain.SelectedIndex = 2;
            tabMain.Refresh();
        }

        private CipherOptions generateOptions(int cipherMode)
        {
            var key = new byte[0];
            if (cbxCipherKeyType.SelectedIndex == 0)
            {
                key = Encoding.Default.GetBytes(txtCipherKey.Text);
            }
            else if (cbxCipherKeyType.SelectedIndex == 1)
            {
                if (!File.Exists(txtCipherKey.Text)) throw new FileNotFoundException(txtCipherKey.Text);
                key = File.ReadAllBytes(txtCipherKey.Text);
            }
            var maskInfoIndex = cbxMaskInformation.SelectedIndex;
            var options = new CipherOptions
            {
                Mode = cipherMode,
                Files = getPathsFromFileList(),
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

        private void chkMaskInformation_CheckedChanged(object sender, EventArgs e)
        {
            cbxMaskInformation.Enabled = chkMaskInformation.Checked;
        }

        private void cbxCipherKeyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCipherKey.UseSystemPasswordChar = cbxCipherKeyType.SelectedIndex == 0;
        }

        private void btnBrowseOrShowKey_Click(object sender, EventArgs e)
        {
            if (cbxCipherKeyType.SelectedIndex == 0)
                txtCipherKey.UseSystemPasswordChar = !txtCipherKey.UseSystemPasswordChar;
            else
                txtCipherKey.Text = browseForKeyFile();
        }

        private static string browseForKeyFile()
        {
            var openFile = new OpenFileDialog();
            if (openFile.ShowDialog() != DialogResult.OK || !openFile.CheckFileExists) return "";
            return openFile.FileName;
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
            string header = "KryptKeeper Status Log" + Environment.NewLine + "Generated on " + timestamp + Environment.NewLine; // TODO INSERT VERSION INFORMATION
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