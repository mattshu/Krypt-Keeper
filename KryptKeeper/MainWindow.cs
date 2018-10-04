/* 
    TODO * MAJOR *
        - IMPERATIVE: * REMOVE HARDCODED KEYFILE *
        - Add Windows context menu options
        - Add option to minimize to tray on close TODO: FIX
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
        private static bool _ExitButtonPressed = false;
        public static Version Version;
        private enum MainTabs
        {
            Options,
            Files,
            Status
        }

        private Status _status;
        private Options _options;
        private bool _settingsNeedConfirmed = true;
        private bool _forceExit;
        private FileList _fileList;

        public MainWindow()
        {
            InitializeComponent();
            _fileList = new FileList(new List<FileData>(), datagridFileList);
        }

        protected internal MetroLabel GetOperationText => lblStatusOperationText;
        protected internal MetroLabel GetFileWorkedText => lblStatusFileWorkedText;
        protected internal MetroLabel GetProcessingRateText => lblStatusProcessingRateText;
        protected internal MetroTextBox GetLogBox => txtStatusLogBox;
        protected internal MetroLabel GetTimeElapsedText => lblStatusTimeElapsedText;
        protected internal MetroLabel GetTimeRemainingText => lblStatusTimeRemainingText;

        #region Refernces of controls for the Options class
        protected internal bool RememberSettings
        {
            get => chkRememberSettings.Checked;
            set => chkRememberSettings.Checked = value;
        }

        protected internal bool MaskFileInformation
        {
            get => chkMaskFileInformation.Checked;
            set => chkMaskFileInformation.Checked = value;
        }

        protected internal bool MaskFileName
        {
            get => chkMaskFileName.Checked;
            set => chkMaskFileName.Checked = value;
        }

        protected internal bool MaskFileDate
        {
            get => chkMaskFileDate.Checked;
            set => chkMaskFileDate.Checked = value;
        }

        protected internal bool RemoveAfterEncryption
        {
            get => chkRemoveAfterEncryption.Checked;
            set => chkRemoveAfterEncryption.Checked = value;
        }

        protected internal bool RemoveAfterDecryption
        {
            get => chkRemoveAfterDecryption.Checked;
            set => chkRemoveAfterDecryption.Checked = value;
        }

        protected internal bool ProcessInOrder
        {
            get => chkProcessInOrder.Checked;
            set => chkProcessInOrder.Checked = value;
        }

        protected internal bool ProcessInOrderDesc
        {
            get => chkProcessInOrderDesc.Checked;
            set => chkProcessInOrderDesc.Checked = value;
        }

        protected internal int ProcessInOrderBy
        {
            get => cbxProcessOrderBy.SelectedIndex;
            set => cbxProcessOrderBy.SelectedIndex = value;
        }

        protected internal bool ConfirmOnExit
        {
            get => chkConfirmOnExit.Checked;
            set => chkConfirmOnExit.Checked = value;
        }

        protected internal bool MinimizeToTrayOnClose
        {
            get => chkMinimizeToTrayOnClose.Checked;
            set => chkMinimizeToTrayOnClose.Checked = value;
        }
        #endregion

        private void mainWindow_Shown(object sender, EventArgs e)
        {
            _status = new Status(this);
            _options = new Options(this);
            _options.Load();
            _fileList.UpdateDataSource();
            setDefaultColumnWidths();
            datagridFileList.DataSourceChanged += datagridFileList_DataSourceChanged;
            chkProcessInOrder.CheckedChanged += chkProcessInOrder_CheckedChanged;
            cbxProcessOrderBy.SelectedIndexChanged += cbxProcessOrderBy_SelectedIndexChanged;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
            Cipher.SetBackgroundWorker(backgroundWorker);
            Version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            lblVersionInformation.Text = Version.ToString();
            buildFileList(DEBUG: true); // TODO DEBUG
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

        // TODO Needs serious refactoring
        private void mainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false;
            if (e.CloseReason == CloseReason.WindowsShutDown || _forceExit)
                return;
            if (!_ExitButtonPressed && chkMinimizeToTrayOnClose.Checked)
            {
                e.Cancel = true;
                Hide();
            }
            if (backgroundWorker.IsBusy)
            {
                e.Cancel = true;
                handleExitWhileBusy();
                return;
            }
            if (_CloseAfterCurrentOperation) return;
            if (!confirmOnExit())
            {
                e.Cancel = true;
                return;
            }
            if (!chkMaskFileName.Checked && !chkMaskFileDate.Checked && chkRemoveAfterDecryption.Checked &&
                chkRemoveAfterEncryption.Checked && radKeyFile.Checked && txtCipherKey.Text.Length == 0 &&
                !chkRememberSettings.Checked)
                e.Cancel = handleSettingsOnExit();
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e) {
            if (chkRememberSettings.Checked)
                _options.Save();
        }

        private void focusTab(MainTabs tab)
        {
            tabMain.SelectedIndex = (int)tab;
        }

        private bool confirmOnExit()
        {
            if (chkConfirmOnExit.Checked)
                return MessageBox.Show(Resources.ExitApplicationMsg, Resources.ExitApplicationTitle,
                           MessageBoxButtons.OKCancel,
                           MessageBoxIcon.Question) == DialogResult.OK;
            return true;
        }

        private bool handleSettingsOnExit()
        {
            switch (MessageBox.Show(Resources.SaveSettingsMsg, Resources.SaveSettingsTitle,
                MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question))
            {
                case DialogResult.Yes:
                    _options.Save();
                    return false;

                case DialogResult.No:
                    _options.Reset();
                    return false;

                default:
                    return true;
            }
        }

        private void handleExitWhileBusy()
        {
            if (_forceExit)
            {
                Cipher.CancelProcessing();
                backgroundWorker.CancelAsync();
            }
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

