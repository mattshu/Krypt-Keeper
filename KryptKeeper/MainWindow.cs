using KryptKeeper.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace KryptKeeper
{
    public partial class MainWindow : Form
    {
        /*
         * TODO *
         * - Fix column resize problem when resizing past border
        */

        private Status _status;

        private static bool CloseAfterCurrentOperation;

        private bool _settingsNeedConfirmed = true;
        private bool _settingsNotViewed = false;

        private List<FileData> _fileList = new List<FileData>();

        private CustomProgressBar _progressCurrent;
        private CustomProgressBar _progressTotal;

        public MainWindow()
        {
            InitializeComponent();
            buildCustomProgressBar();
        }

        private void buildCustomProgressBar()
        {
            _progressCurrent =
                new CustomProgressBar
                {
                    Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right,
                    Location = new Point(6, 6),
                    Name = "_progressCurrent",
                    Size = new Size(353, 23),
                    Step = 1,
                    Maximum = 100
                };
            _progressTotal =
                new CustomProgressBar
                {
                    Anchor = AnchorStyles.Top | AnchorStyles.Right,
                    Location = new Point(365, 6),
                    Name = "_progressTotal",
                    Size = new Size(140, 23),
                    Step = 1,
                    Maximum = 100
                };
            tabPage3.Controls.Add(_progressCurrent);
            tabPage3.Controls.Add(_progressTotal);
        }

        private void mainWindow_Shown(object sender, EventArgs e)
        {
            loadSettings();
            _status = new Status(this);
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            Cipher.SetBackgroundWorker(backgroundWorker);
        }

        private void loadFileListColumnWidths()
        {
            var widths = Settings.Default.fileListColumnWidths;
            for (int i = 0; i < gridFileList.ColumnCount; i++)
                gridFileList.Columns[i].Width = int.Parse(widths[i]);
        }

        private void saveFileListColumnWidths()
        {
            var enumer = gridFileList.Columns.GetEnumerator();
            var widths = new StringCollection();
            while (enumer.MoveNext())
            {
                var header = (DataGridViewTextBoxColumn)enumer.Current;
                if (header == null) break;
                widths.Add(header.Width.ToString());
            }
            if (widths.Count <= 0) return;
            Settings.Default.fileListColumnWidths = widths;
            Settings.Default.Save();
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

        private bool validateSettings()
        {
            if (_fileList.Count <= 0)
            {
                Helper.ShowErrorBox("There are no files to work.");
                focusFilesTab();
                return false;
            }
            if (cbxCipherKeyType.SelectedIndex == 0)
            {
                if (!string.IsNullOrWhiteSpace(txtCipherKey.Text)) return true;
                Helper.ShowErrorBox("The passkey cannot be blank.");
                focusSettingsTab();
                return false;
            }
            if (File.Exists(txtCipherKey.Text))
            {
                if (new FileInfo(txtCipherKey.Text).Length > 0) return true;
                Helper.ShowErrorBox($"Keyfile is empty: {Path.GetFileName(txtCipherKey.Text)}");
                return false;
            }
            Helper.ShowErrorBox($"Unable to find keyfile: {Path.GetFileName(txtCipherKey.Text)}");
            focusSettingsTab();
            return false;
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

        private void setDefaultCbxIndexes()
        {
            cbxMaskInformation.SelectedIndex = 0;
            cbxCipherKeyType.SelectedIndex = 0;
        }

        private void enableControls(bool state)
        {
            btnEncrypt.Enabled = btnDecrypt.Enabled = state;
        }

        private void buildFileList()
        {
            var openFileDialog = new OpenFileDialog { Multiselect = true };
            var openResult = openFileDialog.ShowDialog();
            if (openResult != DialogResult.OK) return;
            gridFileList.Columns.Clear();
            _fileList = openFileDialog.FileNames.Select(path => new FileData(path)).ToList();
            gridFileList.DataSource = _fileList;
            loadFileListColumnWidths();
            enableControls(gridFileList.RowCount > 0);
            tabMain.SelectedIndex = 0;
        }

        private void resetFileList()
        {
            gridFileList.DataSource = null;
        }

        private void refreshFileListGridView()
        {
            gridFileList.DataSource = null;
            gridFileList.DataSource = _fileList;
        }

        private void removeSelectedFiles()
        {
            var selectedCount = gridFileList.SelectedRows.Count;
            if (selectedCount <= 0) return;
            for (int i = gridFileList.RowCount - 1; i >= 0; i--)
                if (gridFileList.Rows[i].Selected)
                    _fileList.RemoveAt(i);
            refreshFileListGridView();
            gridFileList.ClearSelection();
        }

        private void tabMain_TabIndexChanged(object sender, EventArgs e)
        {
            if (tabMain.TabIndex == 1)
                _settingsNotViewed = false;
        }

        private void btnAddFilesOrCancelOperation_Click(object sender, EventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                var dlgConfirmCancel = MessageBox.Show(Resources.AbortOperationDlgMsg,
                    Resources.OperationBusyTitleMsg, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
                if (dlgConfirmCancel == DialogResult.Yes)
                    Cipher.CancelProcessing();
                else if (dlgConfirmCancel == DialogResult.Cancel)
                    return;
                backgroundWorker.CancelAsync();
                btnAddFilesOrCancelOperation.Enabled = false;
            }
            else
                buildFileList();
        }

        private void btnRemoveSelectedFiles_Click(object sender, EventArgs e)
        {
            removeSelectedFiles();
        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            if (confirmSettings())
                processFiles(Cipher.ENCRYPT);
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            if (confirmSettings())
                processFiles(Cipher.DECRYPT);
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
                txtCipherKey.Text = Helper.BrowseFile();
        }

        private void processFiles(int cipherMode)
        {
            if (!validateSettings()) return;
            btnAddFilesOrCancelOperation.Text = @"Cancel";
            focusStatusTab();
            var options = generateOptions(cipherMode);
            Cipher.ProcessFiles(options);
            resetFileList();
        }

        private void focusFilesTab()
        {
            tabMain.SelectedIndex = 0;
            tabMain.Refresh();
        }

        private void focusSettingsTab()
        {
            tabMain.SelectedIndex = 1;
            tabMain.Refresh();
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
                key = Encoding.UTF8.GetBytes(txtCipherKey.Text);
            else if (cbxCipherKeyType.SelectedIndex == 1)
                key = File.ReadAllBytes(txtCipherKey.Text);
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
            return _fileList.Select(file => file.GetFilePath()).ToArray();
        }

        private void fileListGridView_DataSourceChanged(object sender, EventArgs e)
        {
            enableControls(gridFileList.RowCount > 0);
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
            var saveFileDialog = new SaveFileDialog { DefaultExt = "log", Filter = @"Log files(*.log)|*.*", FileName = $"kryptlog-{DateTime.Now:yyMMdd-HHmm}" };
            var dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK || string.IsNullOrWhiteSpace(saveFileDialog.FileName)) return;
            using (var fStream = saveFileDialog.OpenFile())
            {
                var logHeader = Helper.GenerateLogHeader();
                fStream.Write(logHeader, 0, logHeader.Length);
                fStream.Write(Encoding.Default.GetBytes(txtStatus.Text), 0, txtStatus.TextLength);
            }
            _status.WriteLine("Exported log to " + saveFileDialog.FileName);
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (CloseAfterCurrentOperation) return;
            updateProgress(e.ProgressPercentage, (int)e.UserState);
        }

        private void updateProgress(int progressCurrent, int progressTotal)
        {
            _progressCurrent.Value = progressCurrent;
            _progressTotal.Value = progressTotal;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (CloseAfterCurrentOperation)
            {
                Close();
                return;
            }
            _status.WriteLine("Operation finished. " + Cipher.GetElapsedTime());
            btnAddFilesOrCancelOperation.Text = @"Add Files...";
            btnAddFilesOrCancelOperation.Enabled = true;
            updateProgress(0, 100);
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void mainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                e.Cancel = true;
                switch (new ConfirmExitWhileBusyDialog().ShowDialog())
                {
                    case DialogResult.Abort: // Abort and Exit
                        Cipher.CancelProcessing();
                        CloseAfterCurrentOperation = true;
                        backgroundWorker.CancelAsync();
                        break;

                    case DialogResult.Ignore: // Finish and Exit
                        CloseAfterCurrentOperation = true;
                        backgroundWorker.CancelAsync();
                        Hide();
                        break;
                }
            }
            else
            {
                e.Cancel = false;
                if (CloseAfterCurrentOperation)
                    return;
                if (!confirmExit())
                {
                    e.Cancel = true;
                    return;
                }
                saveFileListColumnWidths();
                if (chkRememberSettings.Checked)
                    saveSettings();
                else if (!settingsAreDefault())
                {
                    switch (MessageBox.Show(Resources.SaveSettingsMsg, Resources.SaveSettingsTitle,
                        MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
                    {
                        case DialogResult.Yes:
                            saveSettings();
                            break;

                        case DialogResult.No:
                            Helper.ResetSettings();
                            break;

                        default:
                            e.Cancel = true;
                            break;
                    }
                }
            }
        }

        private bool confirmExit()
        {
            if (chkConfirmOnExit.Checked)
                return MessageBox.Show(Resources.ExitApplicationMsg, Resources.ExitApplicationTitle,
                       MessageBoxButtons.OKCancel,
                       MessageBoxIcon.Question) == DialogResult.OK;
            return true;
        }

        private void gridFileList_SelectionChanged(object sender, EventArgs e)
        {
            btnRemoveFiles.Enabled = gridFileList.SelectedRows.Count > 0;
        }

        public TextBox GetStatusBox()
        {
            return txtStatus;
        }

        public CustomProgressBar GetProgressBarCurrent()
        {
            return _progressCurrent;
        }

        public CustomProgressBar GetProgressBarTotal()
        {
            return _progressTotal;
        }
    }
}