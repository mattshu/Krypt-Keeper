using KryptKeeper.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Windows.Forms;
using MetroFramework;
using MetroFramework.Controls;

namespace KryptKeeper
{
    public partial class MainWindow : MetroFramework.Forms.MetroForm
    {
        /* 
            TODO
                - If planning on storing keys, ensure key storage security
        */
        public const int MINIMUM_PLAINTEXT_KEY_LENGTH = 8;
        public MetroTextBox GetStatusBox() => txtStatus;
        private static bool closeAfterCurrentOperation;
        private enum MainTabs { Options, Files, Status }
        private Status _status;
        private bool _settingsNeedConfirmed = true;
        private List<FileData> _fileList = new List<FileData>();

        public MainWindow()
        {
            InitializeComponent();
        }

        public List<Control> GetStatusObjects()
        {
            return new List<Control>
            {
                lblOperationStatus,
                lblProcessingFile,
                txtStatus
            };
        }

        #region Form Events
        private void mainWindow_Shown(object sender, EventArgs e)
        {
            focusTab(MainTabs.Options);
            loadSettings();
            _status = new Status(this);
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            Cipher.SetBackgroundWorker(backgroundWorker);
        }

        private void chkMaskFileInformation_CheckedChanged(object sender, EventArgs e)
        {
            chkMaskFileName.Enabled = chkMaskFileDate.Enabled = chkMaskFileInformation.Checked;
            if (chkMaskFileInformation.Checked) return;
            chkMaskFileDate.Checked = false;
            chkMaskFileName.Checked = false;
        }

        private void radKeyFile_CheckedChanged(object sender, EventArgs e)
        {
            btnBrowseKeyFile.Enabled = radKeyFile.Checked;
            txtCipherKey.Clear();
            txtCipherKey.ReadOnly = radKeyFile.Checked;
            txtCipherKey.ShowButton = radPlaintextKey.Checked;
            txtCipherKey.UseSystemPasswordChar = radPlaintextKey.Checked;
            if (radKeyFile.Checked)
            {
                txtCipherKey.Text = Helper.BrowseFiles(multiSelect: false);
                txtCipherKey.WaterMark = @"You must browse for a key file...";
            }
            else
            {
                txtCipherKey.WaterMark = "";
            }
        }

        private void txtCipherKey_ButtonClick(object sender, EventArgs e)
        {
            txtCipherKey.UseSystemPasswordChar = !txtCipherKey.UseSystemPasswordChar;
        }

        private void btnBrowseKeyFile_Click(object sender, EventArgs e)
        {
            txtCipherKey.Text = Helper.BrowseFiles(multiSelect: false);
        }

        private void btnSelectFiles_Click(object sender, EventArgs e)
        {
            buildFileList();
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

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

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (closeAfterCurrentOperation) return;
            updateProgress(e.ProgressPercentage, (int)e.UserState);
            lblFilesToBeProcessed.Text = Cipher.GetFileProgress();
            lblTimeElapsed.Text = Cipher.GetElapsedTime(hideMs: true) + @"elapsed";
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (closeAfterCurrentOperation)
            {
                Close();
                return;
            }
            _status.WriteLine("Operation finished. " + Cipher.GetElapsedTime());
            btnCancelOperation.Enabled = false;
            updateProgress(100, 100);
            lblFilesToBeProcessed.Text = e.Cancelled ? "Some" : "All" + " files processed";
            lblProcessingFile.Text = "";
            lblCurrentPercentage.Text = @"100%";
            lblTotalPercentage.Text = @"100%";
            lblOperationStatus.Text = @"Done!";
        }

        private void btnCancelOperation_Click(object sender, EventArgs e)
        {
            if (!backgroundWorker.IsBusy) return;
            var dlgConfirmCancel = MessageBox.Show(Resources.AbortOperationDlgMsg,
                Resources.OperationBusyTitleMsg, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);
            if (dlgConfirmCancel == DialogResult.Yes)
                Cipher.CancelProcessing();
            else if (dlgConfirmCancel == DialogResult.Cancel)
                return;
            backgroundWorker.CancelAsync();
            btnCancelOperation.Enabled = false;
        }

        private void txtStatus_TextChanged(object sender, EventArgs e)
        {
            btnExport.Enabled = txtStatus.Text.Length > 0;
        }

        private void btnExport_Click(object sender, EventArgs e)
        {
            exportStatusLog();
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            txtStatus.Clear();
        }

        private void mainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (backgroundWorker.IsBusy)
            {
                e.Cancel = true;
                handleExitWhileBusy();
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
                    e.Cancel = handleSettingsOnExit();
                }
            }
        }

        #endregion

        private void focusTab(MainTabs tab)
        {
            tabMain.SelectedIndex = (int)tab;
            tabMain.Refresh();
        }

        private void loadSettings()
        {
            progressCurrent.Reset();
            progressTotal.Reset();
            var settings = Settings.Default;
            if (!settings.rememberSettings)
                Helper.ResetSettings();
            chkMaskFileInformation.Checked = settings.encryptionMaskFileName || settings.encryptionMaskFileDate;
            chkMaskFileName.Checked = settings.encryptionMaskFileName;
            chkMaskFileDate.Checked = settings.encryptionMaskFileDate;
            chkRemoveAfterEncryption.Checked = settings.removeAfterEncryption;
            chkRemoveAfterDecryption.Checked = settings.removeAfterDecryption;
            chkRememberSettings.Checked = settings.rememberSettings;
            chkConfirmExit.Checked = settings.confirmOnExit;
        }

        private void enableProcessButtons(bool state)
        {
            btnEncrypt.Enabled = btnDecrypt.Enabled = state;
        }

        private void buildFileList()
        {
            if (!validateKeySettings())
            {
                focusTab(MainTabs.Options);
                if (radKeyFile.Checked)
                    txtCipherKey.Text = Helper.BrowseFiles(false);
                return;
            }
            var openFileDialog = new OpenFileDialog { Multiselect = true };
            var openResult = openFileDialog.ShowDialog();
            if (openResult != DialogResult.OK) return;
            datagridFileList.Columns.Clear();
            _fileList = openFileDialog.FileNames.Select(path => new FileData(path)).ToList();
            datagridFileList.DataSource = _fileList;
            arrangeFileListColumns();
            loadFileListColumnWidths();
            enableProcessButtons(datagridFileList.RowCount > 0);
            lblFileCount.Text = $@"File count: {_fileList.Count}";
            lblPayload.Text = $@"Payload: {Helper.BytesToString(_fileList.CalculateTotalPayloadBytes())}";
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

        private void removeSelectedFiles()
        {
            var selectedCount = datagridFileList.SelectedRows.Count;
            if (selectedCount <= 0) return;
            for (int i = datagridFileList.RowCount - 1; i >= 0; i--)
                if (datagridFileList.Rows[i].Selected)
                    _fileList.RemoveAt(i);
            datagridFileList.ClearSelection();
            refreshFileListGridView();
            arrangeFileListColumns();
            loadFileListColumnWidths();
        }

        private void refreshFileListGridView()
        {
            datagridFileList.DataSource = null;
            datagridFileList.DataSource = _fileList;
        }

        private void processFiles(int cipherMode)
        {
            try
            {
                if (!validateAllSettings()) return;
                focusTab(MainTabs.Status);
                btnCancelOperation.Enabled = true;
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

        private CipherOptions generateOptions(int cipherMode)
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
            lblFileCount.Text = "";
            lblPayload.Text = "";
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

        private void exportStatusLog()
        {
            var saveFileDialog = new SaveFileDialog { DefaultExt = "log", Filter = @"Log files(*.log)|*.*", FileName = $"kryptlog-{DateTime.Now:yyMMdd-HHmm}" };
            var dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK || string.IsNullOrWhiteSpace(saveFileDialog.FileName)) return;
            using (var fStream = saveFileDialog.OpenFile())
            {
                var logHeader = Helper.GenerateLogHeader();
                fStream.Write(logHeader, 0, logHeader.Length);
                fStream.Write(Helper.GetBytes(txtStatus.Text), 0, txtStatus.Text.Length);
            }
            _status.WriteLine("Exported log to " + saveFileDialog.FileName);
        }

        private void updateProgress(int current, int total)
        {
            progressCurrent.Value = current;
            lblCurrentPercentage.Text = $@"{current}%";
            progressTotal.Value = total;
            lblTotalPercentage.Text = $@"{total}%";
        }

        private bool confirmExit()
        {
            if (chkConfirmExit.Checked)
                return MessageBox.Show(Resources.ExitApplicationMsg, Resources.ExitApplicationTitle,
                           MessageBoxButtons.OKCancel,
                           MessageBoxIcon.Question) == DialogResult.OK;
            return true;
        }

        private void saveFileListColumnSettings()
        {
            var widths = new StringCollection();
            var orders = new StringCollection();
            var enumer = datagridFileList.Columns.GetEnumerator();
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
            settings.encryptionMaskFileName = chkMaskFileInformation.Checked && chkMaskFileName.Checked;
            settings.encryptionMaskFileDate = chkMaskFileInformation.Checked && chkMaskFileDate.Checked;
            settings.removeAfterDecryption = chkRemoveAfterDecryption.Checked;
            settings.removeAfterEncryption = chkRemoveAfterEncryption.Checked;
            settings.rememberSettings = true;
            settings.confirmOnExit = chkConfirmExit.Checked;
            settings.Save();
        }

        private bool settingsAreDefault()
        {
            return !chkMaskFileName.Checked && !chkMaskFileDate.Checked && chkRemoveAfterDecryption.Checked &&
                chkRemoveAfterEncryption.Checked && radKeyFile.Checked && txtCipherKey.Text.Length == 0;
        }

        private bool handleSettingsOnExit()
        {
            switch (MessageBox.Show(Resources.SaveSettingsMsg, Resources.SaveSettingsTitle,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    saveSettings();
                    return false;

                case DialogResult.No:
                    Helper.ResetSettings();
                    return false;

                default:
                    return true;
            }
        }

        private void handleExitWhileBusy()
        {
            switch (new ConfirmExitWhileBusy().ShowDialog())
            {
                case DialogResult.Abort: // Abort and Exit
                    Cipher.CancelProcessing();
                    closeAfterCurrentOperation = true;
                    backgroundWorker.CancelAsync();
                    break;

                case DialogResult.Retry: // Finish File and Exit
                    closeAfterCurrentOperation = true;
                    backgroundWorker.CancelAsync();
                    Hide();
                    break;

                case DialogResult.Ignore: // Finish in Background
                    closeAfterCurrentOperation = true;
                    Hide();
                    break;
            }
        }

    }
}
