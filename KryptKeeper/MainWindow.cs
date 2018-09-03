using KryptKeeper.Properties;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace KryptKeeper
{
    public partial class MainWindow : MetroFramework.Forms.MetroForm
    {
        /* 
            TODO * MAJOR *
                - If planning on storing keys, ensure key storage security
                - Add drag and drop capabilities
                - Calculate processing speeds
                - Option: shutdown/sleep/restart after job
                - Always work toward single responsibility principle
                - IMPERATIVE: *** REMOVE HARDCODED KEYFILE *** 
            TODO * MINOR *
                - Option: process files according to size (*in progress)
                - Fix column order
                - Refactor constants to own class?
                - Dialog icons
        */
        public const int MINIMUM_PLAINTEXT_KEY_LENGTH = 8;
        private static bool closeAfterCurrentOperation;
        private enum MainTabs { Options, Files, Status }
        private Status _status;
        private bool _settingsNeedConfirmed = true;
        private FileList _fileList = new FileList();

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

        private void mainWindow_Shown(object sender, EventArgs e)
        {
            focusTab(MainTabs.Options);
            loadSettings();
            _status = new Status(this);
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            Cipher.SetBackgroundWorker(backgroundWorker);
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
            chkProcessInOrder.Checked = settings.processInOrder;
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
