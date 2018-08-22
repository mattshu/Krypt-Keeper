using KryptKeeper.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace KryptKeeper
{
    public partial class MainWindow : Form
    {
        private static bool closeAfterCurrentOperation;
        private bool _settingsNeedConfirmed = true;
        private bool _settingsNotViewed = false;
        private List<FileData> _fileList = new List<FileData>();
        private Status _status;
        private CustomProgressBar _progressCurrent;
        private CustomProgressBar _progressTotal;

        public TextBox GetStatusBox() => txtStatus;
        public CustomProgressBar GetProgressBarCurrent() => _progressCurrent;
        public CustomProgressBar GetProgressBarTotal() => _progressTotal;

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

        private void setDefaultCbxIndexes()
        {
            cbxMaskInformation.SelectedIndex = 0;
            cbxCipherKeyType.SelectedIndex = 0;
        }

        private void fileListDataGridView_SelectionChanged(object sender, EventArgs e)
        {
            btnRemoveFiles.Enabled = fileListDataGridView.SelectedRows.Count > 0;
        }

        private void fileListDataGridView_DataSourceChanged(object sender, EventArgs e)
        {
            enableControls(fileListDataGridView.RowCount > 0);
        }

        private void enableControls(bool state)
        {
            btnEncrypt.Enabled = btnDecrypt.Enabled = state;
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

        private void buildFileList()
        {
            var openFileDialog = new OpenFileDialog { Multiselect = true };
            var openResult = openFileDialog.ShowDialog();
            if (openResult != DialogResult.OK) return;
            fileListDataGridView.Columns.Clear();
            _fileList = openFileDialog.FileNames.Select(path => new FileData(path)).ToList();
            fileListDataGridView.DataSource = _fileList;
            arrangeFileListColumns();
            loadFileListColumnWidths();
            enableControls(fileListDataGridView.RowCount > 0);
            tabMain.SelectedIndex = 0;
        }

        private void arrangeFileListColumns()
        {
            var columnOrderFromSettings = Settings.Default.fileListColumnOrder;
            for (int i = 0; i < columnOrderFromSettings.Count; i++)
                fileListDataGridView.Columns[i].DisplayIndex = int.Parse(columnOrderFromSettings[i]);
        }

        private void loadFileListColumnWidths()
        {
            var widths = Settings.Default.fileListColumnWidths;
            for (int i = 0; i < fileListDataGridView.ColumnCount; i++)
                fileListDataGridView.Columns[i].Width = int.Parse(widths[i]);
        }

        private void btnRemoveSelectedFiles_Click(object sender, EventArgs e)
        {
            removeSelectedFiles();
        }

        private void removeSelectedFiles()
        {
            var selectedCount = fileListDataGridView.SelectedRows.Count;
            if (selectedCount <= 0) return;
            for (int i = fileListDataGridView.RowCount - 1; i >= 0; i--)
                if (fileListDataGridView.Rows[i].Selected)
                    _fileList.RemoveAt(i);
            refreshFileListGridView();
            fileListDataGridView.ClearSelection();
        }

        private void refreshFileListGridView()
        {
            fileListDataGridView.DataSource = null;
            fileListDataGridView.DataSource = _fileList;
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

        private void processFiles(int cipherMode)
        {
            try
            {
                if (!validateSettings()) return;
                btnAddFilesOrCancelOperation.Text = @"Cancel";
                focusStatusTab();
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

        private void focusStatusTab()
        {
            tabMain.SelectedIndex = 2;
            tabMain.Refresh();
        }

        private CipherOptions generateOptions(int cipherMode)
        {
            byte[] key;
            if (cbxCipherKeyType.SelectedIndex == 0)
                key = Helper.GetBytes(txtCipherKey.Text);
            else if (cbxCipherKeyType.SelectedIndex == 1 && File.Exists(txtCipherKey.Text))
            {
                if (new FileInfo(txtCipherKey.Text).Length < Cipher.MAX_FILE_LENGTH)
                    key = File.ReadAllBytes(txtCipherKey.Text);
                else
                    throw new FileLoadException(txtCipherKey.Text);
            }
            else
                throw new FileNotFoundException(txtCipherKey.Text);
            var maskInfoIndex = cbxMaskInformation.SelectedIndex;
            var options = new CipherOptions
            {
                Mode = cipherMode,
                Files = getPathsFromFileList(),
                Key = key,
                Salt = Helper.GetBytes(BCrypt.GenerateSalt()),
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

        private void resetFileList()
        {
            fileListDataGridView.DataSource = null;
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
                fStream.Write(Helper.GetBytes(txtStatus.Text), 0, txtStatus.TextLength);
            }
            _status.WriteLine("Exported log to " + saveFileDialog.FileName);
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (closeAfterCurrentOperation) return;
            updateProgress(e.ProgressPercentage, (int)e.UserState);
        }

        private void updateProgress(int progressCurrent, int progressTotal)
        {
            _progressCurrent.Value = progressCurrent;
            _progressTotal.Value = progressTotal;
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (closeAfterCurrentOperation)
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
                        closeAfterCurrentOperation = true;
                        backgroundWorker.CancelAsync();
                        break;

                    case DialogResult.Ignore: // Finish and Exit
                        closeAfterCurrentOperation = true;
                        backgroundWorker.CancelAsync();
                        Hide();
                        break;
                }
            }
            else
            {
                e.Cancel = false;
                if (closeAfterCurrentOperation)
                    return;
                if (!confirmExit())
                {
                    e.Cancel = true;
                    return;
                }
                saveFileListColumnSettings();
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

        private void saveFileListColumnSettings()
        {
            var widths = new StringCollection();
            var orders = new StringCollection();
            var enumer = fileListDataGridView.Columns.GetEnumerator();
            while (enumer.MoveNext())
            {
                var header = (DataGridViewTextBoxColumn)enumer.Current;
                if (header == null) break;
                widths.Add(header.Width.ToString());
                orders.Add(header.DisplayIndex.ToString());
            }
            if (widths.Count <= 0) return;
            Settings.Default.fileListColumnWidths = widths;
            Settings.Default.fileListColumnOrder = orders;
            Settings.Default.Save();
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

        private bool settingsAreDefault()
        {
            if (_settingsNotViewed) return true;
            return !txtCipherKey.Modified;
        }
    }
}