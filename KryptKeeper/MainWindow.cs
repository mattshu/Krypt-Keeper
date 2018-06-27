using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;
using System.Xml;

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

        private void LoadFileListColumnWidths()
        {
            var widths = Properties.Settings.Default.fileListColumnWidths;
            for (int i = 0; i < FileListGridView.ColumnCount; i++)
                FileListGridView.Columns[i].Width = int.Parse(widths[i]);
        }

        private void MainWindow_Shown(object sender, EventArgs e)
        {
            LoadSettings();
            Encryptor.AES();
        }

        private void LoadSettings()
        {
            var settings = Properties.Settings.Default;

            var algorithms = settings.algorithms;

            foreach (var a in algorithms)
            {
                CbxEncryptAlgorithms.Items.Add(a);
                CbxDecryptAlgorithms.Items.Add(a);
            }

            CbxEncryptAlgorithms.SelectedIndex = settings.encryptionAlgorithm;
            ChkMaskInformation.Checked = settings.encryptionMaskInformation;
            CbxMaskInformation.SelectedIndex = settings.encryptionMaskInfoType;
            ChkRemoveAfterEncrypt.Checked = settings.encryptionRemoveAfterEncrypt;
            CbxEncryptionKeyType.SelectedIndex = settings.encryptionKeyType;
            TxtEncryptionKey.Text = settings.encryptionKey;

            if (!SettingsAreDefault()) ChkRememberSettings.Checked = true;

            if (settings.useEncryptionSettings)
                CopyEncryptionSettings();

            else
            {
                ChkUseEncryptSettings.Checked = false;
                CbxDecryptionKeyType.Enabled = true;
                CbxDecryptAlgorithms.Enabled = true;
                TxtDecryptionKey.ReadOnly = false;

                CbxDecryptAlgorithms.SelectedIndex = settings.decryptionAlgorithm;
                CbxDecryptionKeyType.SelectedIndex = settings.decryptionKeyType;
                TxtDecryptionKey.Text = settings.decryptionKey;
            }

        }

        private void BtnAddFiles_Click(object sender, EventArgs e)
        {
            AddFilesToList();
        }

        private void AddFilesToList()
        {
            var openFileDialog = new OpenFileDialog { Multiselect = true };
            var openResult = openFileDialog.ShowDialog();
            if (openResult != DialogResult.OK) return;
            
            FileListGridView.Columns.Clear();
            _fileList = openFileDialog.FileNames.Select(path => new FileData(path)).ToList();
            FileListGridView.DataSource = _fileList;
            LoadFileListColumnWidths();
            EnableControls(FileListGridView.RowCount > 0);
        }


        private void EnableControls(bool state)
        {
            BtnEncrypt.Enabled = BtnDecrypt.Enabled = state;
        }

        private void BtnRemoveFiles_Click(object sender, EventArgs e)
        {
            RemoveSelectedFiles();
        }

        private void RemoveSelectedFiles()
        {
            int selectedCount = FileListGridView.SelectedRows.Count;
            if (selectedCount <= 0) return;
            for (int i = FileListGridView.RowCount - 1; i >= 0; i--)
                if (FileListGridView.Rows[i].Selected)
                    _fileList.RemoveAt(i);
            RefreshFileListGridView();
            FileListGridView.ClearSelection();
        }

        private void RefreshFileListGridView()
        {
            FileListGridView.DataSource = null;
            FileListGridView.DataSource = _fileList;
        }

        private bool SettingsAreDefault()
        {
            if (_settingsNotViewed) return true;
            bool settingsModified = TxtEncryptionKey.Modified || TxtDecryptionKey.Modified ||
                                    CbxEncryptAlgorithms.SelectedIndex > -1 ||
                                    CbxDecryptAlgorithms.SelectedIndex > -1 ||
                                    CbxEncryptionKeyType.SelectedIndex > -1 ||
                                    CbxDecryptionKeyType.SelectedIndex > -1;
            return !settingsModified;
        }

        private void TabMain_TabIndexChanged(object sender, EventArgs e)
        {
            if (TabMain.TabIndex == 1)
                _settingsNotViewed = false;
        }

        private void BtnEncrypt_Click(object sender, EventArgs e)
        {
            if (SettingsAreDefault() || _settingsNeedConfirmed)
            {
                var confirmSettingsDialog = new ConfirmSettingsDialog();
                var confirmSettingsResult = confirmSettingsDialog.ShowDialog();
                _settingsNeedConfirmed = confirmSettingsDialog.ShowAgain;
                if (confirmSettingsResult == DialogResult.No)
                {
                    TabMain.SelectTab(1);
                    return;
                }
            }
            BeginEncryption();
        }

        private void BeginEncryption()
        {
            // TODO 
            throw new NotImplementedException();
        }

        private void BtnDecrypt_Click(object sender, EventArgs e)
        {
            // TODO 

        }

        private void CbxEncryptAlgorithms_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ChkUseEncryptSettings.Checked)
                CbxDecryptAlgorithms.SelectedIndex = CbxEncryptAlgorithms.SelectedIndex;
        }

        private void ChkMaskInformation_CheckedChanged(object sender, EventArgs e)
        {
            CbxMaskInformation.Enabled = ChkMaskInformation.Checked;
        }

        private void ChkUseEncryptSettings_CheckedChanged(object sender, EventArgs e)
        {
            if (ChkUseEncryptSettings.Checked) CopyEncryptionSettings();
            CbxDecryptAlgorithms.Enabled = !ChkUseEncryptSettings.Checked;
            CbxDecryptionKeyType.Enabled = !ChkUseEncryptSettings.Checked;
            TxtDecryptionKey.ReadOnly = ChkUseEncryptSettings.Checked;
        }

        private void CopyEncryptionSettings()
        {
            CbxDecryptAlgorithms.SelectedIndex = CbxEncryptAlgorithms.SelectedIndex;
            CbxDecryptionKeyType.SelectedIndex = CbxEncryptionKeyType.SelectedIndex;
            TxtDecryptionKey.Text = TxtEncryptionKey.Text;
            BtnBrowseDecrypt.Enabled = BtnBrowseEncrypt.Enabled;
        }

        private void CbxEncryptionKeyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFormBasedOnKeyType(CbxEncryptionKeyType, BtnBrowseEncrypt, TxtEncryptionKey);
            if (ChkUseEncryptSettings.Checked)
                CbxDecryptionKeyType.SelectedIndex = CbxEncryptionKeyType.SelectedIndex;
        }

        private void TxtEncryptionKey_TextChanged(object sender, EventArgs e)
        {
            if (ChkUseEncryptSettings.Checked)
                TxtDecryptionKey.Text = TxtEncryptionKey.Text;
        }

        private void CbxDecryptionKeyType_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateFormBasedOnKeyType(CbxDecryptionKeyType, BtnBrowseDecrypt, TxtDecryptionKey);
        }

        private static void UpdateFormBasedOnKeyType(ListControl cbxKeyType, Control btnBrowse, TextBox txtKey)
        {
            btnBrowse.Enabled = cbxKeyType.SelectedIndex == 1; // Key file
            if (cbxKeyType.SelectedIndex == 1) txtKey.Text = "";
        }

        private void BtnBrowseEncrypt_Click(object sender, EventArgs e)
        {
            TxtEncryptionKey.Text = BrowseForKeyFile();
        }

        private void BtnBrowseDecrypt_Click(object sender, EventArgs e)
        {
            TxtDecryptionKey.Text = BrowseForKeyFile();
        }

        private void FileList_SelectionChanged(object sender, EventArgs e)
        {
            BtnRemoveFiles.Enabled = BtnEncryptSelected.Enabled = BtnDecryptSelected.Enabled = FileListGridView.SelectedRows.Count > 0;
        }

        private static string BrowseForKeyFile()
        {
            var openFile = new OpenFileDialog();
            if (openFile.ShowDialog() != DialogResult.OK || !openFile.CheckFileExists) return "";
            return openFile.FileName;
        }

        private void BtnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void TxtStatus_TextChanged(object sender, EventArgs e)
        {
            BtnExport.Enabled = TxtStatus.Text.Length > 0;
        }

        private void MainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            SaveFileListColumnWidths();

            if (SettingsAreDefault()) return;

            if (ChkRememberSettings.Checked)
            {
                SaveSettings();
                return;
            }

            var result = MessageBox.Show(@"Do you want to save your settings?", @"Save Settings?", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
                SaveSettings();
            else if (result == DialogResult.Cancel)
                e.Cancel = true;
            else
                ResetSettings();
        }

        private void SaveFileListColumnWidths()
        {
            var enumer = FileListGridView.Columns.GetEnumerator();
            var widths = new StringCollection();
            while (enumer.MoveNext())
            {
                var header = (DataGridViewTextBoxColumn)enumer.Current;
                if (header == null) break;
                widths.Add(header.Width.ToString());
            }
            if (widths.Count <= 0) return;
            Properties.Settings.Default.fileListColumnWidths = widths;
            Properties.Settings.Default.Save();
        }

        private void SaveSettings()
        {
            var settings = Properties.Settings.Default;

            settings.encryptionAlgorithm = CbxEncryptAlgorithms.SelectedIndex;
            settings.encryptionKey = TxtEncryptionKey.Text;
            settings.encryptionKeyType = CbxEncryptionKeyType.SelectedIndex;

            settings.encryptionMaskInformation = ChkMaskInformation.Checked;
            settings.encryptionMaskInfoType = CbxMaskInformation.SelectedIndex;

            settings.useEncryptionSettings = ChkUseEncryptSettings.Checked;

            settings.decryptionAlgorithm = CbxDecryptAlgorithms.SelectedIndex;
            settings.decryptionKey = TxtDecryptionKey.Text;
            settings.decryptionKeyType = CbxDecryptionKeyType.SelectedIndex;

            settings.Save();
        }

        private static void ResetSettings()
        {
            var settings = Properties.Settings.Default;

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

        private void FileListGridView_DataSourceChanged(object sender, EventArgs e)
        {
            EnableControls(FileListGridView.RowCount > 0);
        }
    }
}
