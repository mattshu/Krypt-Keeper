using KryptKeeper.Properties;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace KryptKeeper
{
    public partial class MainWindow : MetroFramework.Forms.MetroForm
    {
        /* 
            TODO * MAJOR * (IMPERATIVE: *** REMOVE HARDCODED KEYFILE ***)
                - Option: shutdown/sleep/restart after job
            TODO * MINOR *
                - Dialog icons
                - Calculate processing speeds
                - If planning on storing keys, ensure key storage security
                - Always work toward single responsibility principle
        */
        private static bool _CloseAfterCurrentOperation;
        private enum MainTabs { Options, Files, Status }
        private Status _status;
        private bool _settingsNeedConfirmed = true;
        private bool _forceExit = false;
        private FileList _fileList;

        public MainWindow()
        {
            InitializeComponent();
            _fileList = new FileList(new List<FileData>(), datagridFileList);
        }
        
        public List<Control> GetStatusObjects()
        {
            return new List<Control>
            {
                lblOperationStatus,
                lblProcessingFile,
                lblProcessRates,
                txtStatus
            };
        }

        private void mainWindow_Shown(object sender, EventArgs e)
        {
            focusTab(MainTabs.Options);
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
        }

        private void mainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_forceExit)
            {
                if (chkRememberSettings.Checked)
                    saveSettings();
            }
            if (backgroundWorker.IsBusy)
            {
                e.Cancel = true;
                handleExitWhileBusy();
            }
            else
            {
                e.Cancel = false;
                if (_CloseAfterCurrentOperation)
                    return;
                if (!confirmExit())
                {
                    e.Cancel = true;
                    return;
                }
                if (chkRememberSettings.Checked)
                    saveSettings();
                else if (!settingsAreDefault())
                {
                    e.Cancel = handleSettingsOnExit();
                }
            }
        }

        private void focusTab(MainTabs tab)
        {
            tabMain.SelectedIndex = (int)tab;
        }

        private void loadSettings()
        {
            progressCurrent.Reset();
            progressTotalFiles.Reset();
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
            chkConfirmExit.Checked = settings.confirmOnExit;
        }

        private bool confirmExit()
        {
            if (chkConfirmExit.Checked)
                return MessageBox.Show(Resources.ExitApplicationMsg, Resources.ExitApplicationTitle,
                           MessageBoxButtons.OKCancel,
                           MessageBoxIcon.Question) == DialogResult.OK;
            return true;
        }

        private void saveSettings()
        {
            var settings = Settings.Default;
            settings.encryptionMaskFileName = chkMaskFileInformation.Checked && chkMaskFileName.Checked;
            settings.encryptionMaskFileDate = chkMaskFileInformation.Checked && chkMaskFileDate.Checked;
            settings.removeAfterDecryption = chkRemoveAfterDecryption.Checked;
            settings.removeAfterEncryption = chkRemoveAfterEncryption.Checked;
            settings.processInOrder = chkProcessInOrder.Checked;
            settings.processInOrderBy = cbxProcessOrderBy.SelectedIndex;
            settings.rememberSettings = true; // always true in case user exits with saving
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

        private void datagridFileList_DataError(object sender, DataGridViewDataErrorEventArgs e) {
            Console.WriteLine(@"Error has occured: " + e.Exception.Message + @"sender as DataGridView.RowCount:" + (sender as DataGridView)?.RowCount);
        }

        private void panelIconShutdown_Click(object sender, EventArgs e)
        {
            toggleOnCompleteActiveIcon("shutdown");
        }

        private void panelIconRestart_Click(object sender, EventArgs e)
        {
            toggleOnCompleteActiveIcon("restart");
        }

        private void panelIconSleep_Click(object sender, EventArgs e)
        {
            toggleOnCompleteActiveIcon("sleep");
        }

        private void panelIconClose_Click(object sender, EventArgs e) {
            toggleOnCompleteActiveIcon("close");
        }

        private void toggleOnCompleteActiveIcon(string iconName)
        {
            panelIconShutdown.BackgroundImage = iconName == "shutdown" ? Resources.shutdown_active : Resources.shutdown;
            panelIconRestart.BackgroundImage = iconName == "restart" ? Resources.restart_active : Resources.restart;
            panelIconSleep.BackgroundImage = iconName == "sleep" ? Resources.sleep_active : Resources.sleep;
            panelIconClose.BackgroundImage = iconName == "close" ? Resources.close_active : Resources.close;
        }

        private void toggleOnCompleteDisabled(bool state)
        {
            panelIconShutdown.BackgroundImage = state ? Resources.shutdown : Resources.shutdown_disabled;
            panelIconRestart.BackgroundImage = state ? Resources.restart : Resources.restart_disabled;
            panelIconSleep.BackgroundImage = state ? Resources.sleep : Resources.sleep_disabled;
            panelIconClose.BackgroundImage = state ? Resources.close : Resources.close_disabled;
        }

        private void chkOnCompletion_CheckedChanged(object sender, EventArgs e)
        {
            panelIconShutdown.Enabled = panelIconRestart.Enabled =
                panelIconSleep.Enabled = panelIconClose.Enabled = chkOnCompletion.Checked;
            toggleOnCompleteDisabled(chkOnCompletion.Checked);
        }
    }
}
