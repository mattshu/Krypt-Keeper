/* 
    TODO * MAJOR *
        - IMPERATIVE: * REMOVE HARDCODED KEYFILE *
        - Add Windows context menu options
        - Needs work when closing while busy
    TODO * MINOR *
        - Ensure notification of failure to process if exited without task completion
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

        private void mainWindow_FormClosing(object sender, FormClosingEventArgs e)
        {
            e.Cancel = false; // Reset previous cancellation
            if (backgroundWorker.IsBusy)
            {
                e.Cancel = true; // Hold off on closing so user can decide to abort or finish
                var unexpectedExit = e.CloseReason == CloseReason.TaskManagerClosing ||
                                e.CloseReason == CloseReason.WindowsShutDown; // Unless someone upstairs wants us to stop
                if (unexpectedExit || _forceExit) // Or if we've been here before, just exit
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
                return; // Close
            if (_ExitButtonPressed)
            {
                _ExitButtonPressed = false;
                e.Cancel = chkConfirmOnExit.Checked && !confirmOnExit();
            }
            else
            {
                if (!MinimizeToTrayOnClose)
                    e.Cancel = chkConfirmOnExit.Checked && !confirmOnExit();
                else
                {
                    e.Cancel = true;
                    Hide();
                }
            }
        }

        private void cancelAllOperations(bool closeAfterwards = false)
        {
            _CloseAfterCurrentOperation = closeAfterwards;
            Cipher.CancelProcessing();
            backgroundWorker.CancelAsync();
        }

        private void MainWindow_FormClosed(object sender, FormClosedEventArgs e) {
            if (chkRememberSettings.Checked)
                _options.Save();
            systemTrayIcon.Dispose();
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
    }
}