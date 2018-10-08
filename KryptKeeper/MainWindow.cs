/* 
    TODO * MAJOR *
        - IMPERATIVE: * REMOVE HARDCODED KEYFILE *
        - Add Windows context menu options
    TODO * MINOR *
        - Tooltips on completion or error if app is minimized to tray
        - If planning on storing keys, ensure key storage security
        - Always work toward single responsibility principle
*/
using KryptKeeper.Properties;
using System;
using System.Collections.Generic;
using System.Windows.Forms;
using MetroFramework;
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

        protected internal MetroLabel GetOperationLabel() => lblStatusOperationText;
        protected internal MetroLabel GetFileWorkedLabel() => lblStatusFileWorkedText;
        protected internal MetroLabel GetProcessingRateLabel() => lblStatusProcessingRateText;
        protected internal MetroTextBox GetLogBox() => txtStatusLogBox;
        protected internal MetroLabel GetTimeElapsedLabel() => lblStatusTimeElapsedText;
        protected internal MetroLabel GetTimeRemainingLabel() => lblStatusTimeRemainingText;

        protected internal void SetLastFileWorked(string file)
        {
            Settings.Default.lastFileWorked = file;
            Settings.Default.Save();
        }

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
            checkIfLastExitSuccess();
            _options = new Options(this);
            _options.Load();
            chkMaskFileInformation.Checked = chkMaskFileName.Enabled = chkMaskFileDate.Enabled = MaskFileInformation;
            _fileList.UpdateDataSource();
            setDefaultColumnWidths();
            setInitialControlEvents();
            Cipher.SetBackgroundWorker(backgroundWorker);
            Version = System.Reflection.Assembly.GetEntryAssembly().GetName().Version;
            lblVersionInformation.Text = Version.ToString();
        }

        private void setInitialControlEvents()
        {
            datagridFileList.DataSourceChanged += datagridFileList_DataSourceChanged;
            chkProcessInOrder.CheckedChanged += chkProcessInOrder_CheckedChanged;
            cbxProcessOrderBy.SelectedIndexChanged += cbxProcessOrderBy_SelectedIndexChanged;
            backgroundWorker.ProgressChanged += backgroundWorker_ProgressChanged;
            backgroundWorker.RunWorkerCompleted += backgroundWorker_RunWorkerCompleted;
        }

        private void checkIfLastExitSuccess()
        {
            var lastExitSuccess = Settings.Default.lastExitSuccess;
            var lastFileWorked = Settings.Default.lastFileWorked;
            if (!lastExitSuccess)
            {
                if (!string.IsNullOrEmpty(lastFileWorked))
                    lastFileWorked = "Please verify the condition of this file:\n" + lastFileWorked + "\n\n";
                MetroMessageBox.Show(this,
                    "Krypt Keeper was interrupted during operation.\n\n" + lastFileWorked +
                    "Remember to exit properly to avoid possible data loss.", "Unexpected Exit", MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            setLastExitSuccess(reset: true); // Reset bad exit flag
        }

        private void menuItemOpen_Click(object sender, EventArgs e)
        {
            restoreWindow();
        }

        private void menuItemExit_Click(object sender, EventArgs e)
        {
            restoreWindow();
            _ExitButtonPressed = true;
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
            e.Cancel = false; // Reset previous cancellation
            var unexpectedExit = e.CloseReason == CloseReason.TaskManagerClosing || 
                                 e.CloseReason == CloseReason.WindowsShutDown;
            if (backgroundWorker.IsBusy)
            {
                e.Cancel = true; // Hold off on closing so user can decide to abort or finish
                if (unexpectedExit || _forceExit)
                    cancelAllOperations(closeAfterwards: true);
                else
                {
                    var confirmStop = confirmStopWhileBusy();
                    if (confirmStop == DialogResult.Abort)
                        cancelAllOperations(closeAfterwards: true);
                    else if (confirmStop == DialogResult.Ignore) // Finish
                    {
                        _status.SetOperationText("Finishing up...");
                        _CloseAfterCurrentOperation = true;
                    }
                }
                return; // Close event is cancelled
            }
            if (_forceExit || _CloseAfterCurrentOperation)
                return; // Forced close
            if (_ExitButtonPressed) // Normal close
            {
                _ExitButtonPressed = false;
                e.Cancel = chkConfirmOnExit.Checked && !confirmOnExit(); // Final confirmation in this case
            }
            else // X button pressed
            {
                if (MinimizeToTrayOnClose)
                {
                    e.Cancel = true;
                    Hide();
                }
                else
                    e.Cancel = chkConfirmOnExit.Checked && !confirmOnExit(); // Final confirmation
            }
        }

        private void cancelAllOperations(bool closeAfterwards = false)
        {
            _CloseAfterCurrentOperation = closeAfterwards;
            Cipher.CancelProcessing();
            backgroundWorker.CancelAsync();
        }

        private void mainWindow_FormClosed(object sender, FormClosedEventArgs e) {
            if (chkRememberSettings.Checked)
                _options.Save();
            else
                _options.Reset();
            systemTrayIcon.Dispose();
            setLastExitSuccess();
        }

        private static void setLastExitSuccess(bool reset = false)
        {
            Settings.Default.lastExitSuccess = !reset;
            Settings.Default.lastFileWorked = "";
            Settings.Default.Save();
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

        private static DialogResult confirmStopWhileBusy() => new ConfirmStopWhileBusy().ShowDialog();

        private void contextMenu_Opening(object sender, System.ComponentModel.CancelEventArgs e)
        {
            menuItemStatus.Text = @"Status: " + _status.GetCurrentStatus();
        }
    }
}