/* 
    TODO * MAJOR *
        - IMPERATIVE: * REMOVE HARDCODED KEYFILE *
        - Add Windows context menu options
        - Add option to minimize to tray on close
        - Dialog icons
        - Add version information to encrypted files for backwards compatibility
    TODO * MINOR *
        - Tooltips on completion or error if app is minimized to tray
        - If planning on storing keys, ensure key storage security
        - Always work toward single responsibility principle
*/
using KryptKeeper.Properties;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MetroFramework.Controls;

namespace KryptKeeper
{
    public partial class MainWindow : MetroFramework.Forms.MetroForm
    {
        private static bool _CloseAfterCurrentOperation;
        private enum MainTabs { Options, Files, Status }
        private Status _status;
        private bool _settingsNeedConfirmed = true;
        private bool _forceExit;
        private FileList _fileList;

        public MainWindow()
        {
            InitializeComponent();
            _fileList = new FileList(new List<FileData>(), datagridFileList);
        }

        public MetroLabel GetOperationText => lblStatusOperationText;
        public MetroLabel GetFileWorkedText => lblStatusFileWorkedText;
        public MetroLabel GetProcessingRateText => lblStatusProcessingRateText;
        public MetroTextBox GetLogBox => txtStatusLogBox;
        public MetroLabel GetTimeElapsedText => lblStatusTimeElapsedText;
        public MetroLabel GetTimeRemainingText => lblStatusTimeRemainingText;

        private void mainWindow_Shown(object sender, EventArgs e)
        {
            loadSettings();
            _status = new Status(this);
            _fileList.UpdateDataSource();
            setDefaultColumnWidths();
            datagridFileList.DataSourceChanged += datagridFileList_DataSourceChanged;
            chkProcessInOrder.CheckedChanged += chkProcessInOrder_CheckedChanged;
            cbxProcessOrderBy.SelectedIndexChanged += cbxProcessOrderBy_SelectedIndexChanged;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            Cipher.SetBackgroundWorker(backgroundWorker);
            buildFileList(DEBUG: true); // TODO DEBUG
        }

        private void MainWindow_Resize(object sender, EventArgs e) {
            if (WindowState != FormWindowState.Minimized) return;
            Hide();
            systemTrayIcon.Visible = true;
        }

        private void menuItemOpen_Click(object sender, EventArgs e)
        {
            restoreWindow();
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void systemTrayIcon_DoubleClick(object sender, EventArgs e)
        {
            restoreWindow();
        }

        private void restoreWindow()
        {
            Show();
            WindowState = FormWindowState.Normal;
        }

        private void mainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
            if (_forceExit)
            {
                saveSettings();
                return;
            }
            if (backgroundWorker.IsBusy)
            {
                e.Cancel = true;
                handleExitWhileBusy();
                return;
            }
            if (_CloseAfterCurrentOperation)
                return;
            if (!confirmOnExit())
            {
                e.Cancel = true;
                return;
            }
            saveSettings();
            if (!settingsAreDefault() && !chkRememberSettings.Checked)
                e.Cancel = handleSettingsOnExit();
        }

        private void focusTab(MainTabs tab)
        {
            tabMain.SelectedIndex = (int)tab;
        }

        private void loadSettings()
        {
            progressCurrent.Reset();
            progressTotalBytes.Reset();
            var settings = Settings.Default;
            if (!settings.rememberSettings)
                Utils.ResetSettings();
            chkMaskFileInformation.Checked = settings.encryptionMaskFileName || settings.encryptionMaskFileDate;
            chkMaskFileName.Checked = settings.encryptionMaskFileName;
            chkMaskFileDate.Checked = settings.encryptionMaskFileDate;
            chkProcessInOrder.Checked = settings.processInOrder;
            chkProcessOrderDesc.Enabled = cbxProcessOrderBy.Enabled = chkProcessInOrder.Checked;
            cbxProcessOrderBy.SelectedIndex = settings.processInOrderBy;
            chkProcessOrderDesc.Checked = settings.processInOrderDesc;
            chkRemoveAfterEncryption.Checked = settings.removeAfterEncryption;
            chkRemoveAfterDecryption.Checked = settings.removeAfterDecryption;
            chkRememberSettings.Checked = settings.rememberSettings;
            chkConfirmOnExit.Checked = settings.confirmOnExit;
        }

        private bool confirmOnExit()
        {
            if (chkConfirmOnExit.Checked)
                return MessageBox.Show(Resources.ExitApplicationMsg, Resources.ExitApplicationTitle,
                           MessageBoxButtons.OKCancel,
                           MessageBoxIcon.Question) == DialogResult.OK;
            return true;
        }

        private void saveSettings()
        {
            if (!chkRememberSettings.Checked) return;
            var settings = Settings.Default;
            settings.encryptionMaskFileName = chkMaskFileInformation.Checked && chkMaskFileName.Checked;
            settings.encryptionMaskFileDate = chkMaskFileInformation.Checked && chkMaskFileDate.Checked;
            settings.removeAfterDecryption = chkRemoveAfterDecryption.Checked;
            settings.removeAfterEncryption = chkRemoveAfterEncryption.Checked;
            settings.processInOrder = chkProcessInOrder.Checked;
            settings.processInOrderBy = cbxProcessOrderBy.SelectedIndex;
            settings.rememberSettings = true; // always true in case user exits with saving
            settings.confirmOnExit = chkConfirmOnExit.Checked;
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
                    Utils.ResetSettings();
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
                    _CloseAfterCurrentOperation = true;
                    backgroundWorker.CancelAsync();
                    break;

                case DialogResult.Retry: // Finish File and Exit
                    _CloseAfterCurrentOperation = true;
                    backgroundWorker.CancelAsync();
                    Hide();
                    break;

                case DialogResult.Ignore: // Finish in Background
                    _CloseAfterCurrentOperation = true;
                    Hide();
                    break;
            }
        }

    }
}
