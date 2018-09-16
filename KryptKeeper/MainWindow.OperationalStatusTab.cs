﻿using System;
using System.ComponentModel;
using System.Windows.Forms;
using KryptKeeper.Properties;

namespace KryptKeeper
{
    public partial class MainWindow
    {
        #region Operational Status Tab Form Events

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (_CloseAfterCurrentOperation) return;
            updateProgress((ProgressPacket)e.UserState);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (_CloseAfterCurrentOperation)
            {
                Close();
                return;
            }
            _status.WriteLine("Operation finished. " + Cipher.GetElapsedTime());
            disableButtonsDuringOperation(false);
            updateProgress();
            lblTotalFiles.Text = e.Cancelled ? "Some" : "All" + " files processed";
            lblJobInformation.Text = "";
            lblFileBeingProcessed.Text = "";
            lblOperationStatus.Text = @"Done!";
            _fileList.Reset();
            _status.StopProcessRateCollection();
            if (!chkOnCompletion.Checked) return;
            if (!validateOnCompletionIconsHaveOneSelection()) return;
            handleOnCompletion();
        }

        private void chkOnCompletion_CheckedChanged(object sender, EventArgs e)
        {
            panelIconShutdown.Enabled = panelIconRestart.Enabled =
                panelIconSleep.Enabled = panelIconClose.Enabled = chkOnCompletion.Checked;
            toggleOnCompleteDisabled(chkOnCompletion.Checked);
        }

        private void panelIconShutdown_Click(object sender, EventArgs e)
        {
            toggleOnCompleteActiveIcon("shutdown");
            toggleOnCompleteTags("shutdown");
        }

        private void panelIconRestart_Click(object sender, EventArgs e)
        {
            toggleOnCompleteActiveIcon("restart");
            toggleOnCompleteTags("restart");
        }

        private void panelIconSleep_Click(object sender, EventArgs e)
        {
            toggleOnCompleteActiveIcon("sleep");
            toggleOnCompleteTags("sleep");
        }

        private void panelIconClose_Click(object sender, EventArgs e)
        {
            toggleOnCompleteActiveIcon("close");
            toggleOnCompleteTags("close");
        }

        private void btnSelectFilesFromStatusTab_Click(object sender, EventArgs e)
        {
            buildFileList();
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

        private void btnExit_Click(object sender, EventArgs e)
        {
            Close();
        }

        #endregion

        private void handleOnCompletion()
        {
            if ((bool)panelIconShutdown.Tag)
            {
                Utils.ShutdownComputer();
                forceExit();
            }
            else if ((bool)panelIconRestart.Tag)
            {
                Utils.RestartComputer();
                forceExit();
            }
            else if ((bool)panelIconSleep.Tag)
            {
                _status.WriteLine("Computer going to sleep...");
                Utils.StandyComputer();
            }
            else if ((bool)panelIconClose.Tag) 
            {
                forceExit();
            }
        }

        private void forceExit()
        {
            _forceExit = true;
            Close();
        }

        private bool validateOnCompletionIconsHaveOneSelection()
        {
            var shutdownOption = bool.Parse(panelIconShutdown.Tag.ToString());
            var restartOption = bool.Parse(panelIconRestart.Tag.ToString());
            var sleepOption = bool.Parse(panelIconSleep.Tag.ToString());
            var closeOption = bool.Parse(panelIconClose.Tag.ToString());
            return shutdownOption || restartOption || sleepOption || closeOption;
        }

        private void toggleOnCompleteActiveIcon(string iconName)
        {
            panelIconShutdown.BackgroundImage = iconName == "shutdown" ? Resources.shutdown_active : Resources.shutdown;
            panelIconRestart.BackgroundImage = iconName == "restart" ? Resources.restart_active : Resources.restart;
            panelIconSleep.BackgroundImage = iconName == "sleep" ? Resources.sleep_active : Resources.sleep;
            panelIconClose.BackgroundImage = iconName == "close" ? Resources.close_active : Resources.close;
        }

        private void toggleOnCompleteTags(string iconName)
        {
            panelIconShutdown.Tag = iconName == "shutdown";
            panelIconRestart.Tag = iconName == "restart";
            panelIconSleep.Tag = iconName == "sleep";
            panelIconClose.Tag = iconName == "close";
        }

        private void toggleOnCompleteDisabled(bool state)
        {
            panelIconShutdown.BackgroundImage = state ? Resources.shutdown : Resources.shutdown_disabled;
            panelIconRestart.BackgroundImage = state ? Resources.restart : Resources.restart_disabled;
            panelIconSleep.BackgroundImage = state ? Resources.sleep : Resources.sleep_disabled;
            panelIconClose.BackgroundImage = state ? Resources.close : Resources.close_disabled;
        }

        private void updateProgress(ProgressPacket packet = null)
        {
            if (packet == null)
            {
                lblCurrentPercentage.Text = @"100%";
                lblTotalBytesPercentage.Text = @"100%";
                progressCurrent.Value = 100;
                progressTotalFiles.Value = 100;
                progressTotalBytes.Value = 100;
                return;
            }
            var currentFileProgress = packet.GetCurrentFileProgress();
            var totalBytesProgress = packet.GetTotalBytesProgress();
            var totalFileProgress = packet.GetTotalFilesProgress();
            progressCurrent.Value = Utils.Clamp(currentFileProgress, 0, 100);
            lblCurrentPercentage.Text = $@"{currentFileProgress}%";
            progressTotalFiles.Value = Utils.Clamp(totalFileProgress, 0, 100);
            progressTotalBytes.Value = Utils.Clamp(totalBytesProgress, 0, 100);
            lblTotalBytesPercentage.Text = $@"{totalBytesProgress}%";
            lblTotalFiles.Text = Cipher.GetFileProgress();
            lblTimeElapsed.Text = Cipher.GetElapsedTime(hideMs: true) + @"elapsed";
        }

        private void exportStatusLog()
        {
            var saveFileDialog = new SaveFileDialog { DefaultExt = "log", Filter = @"Log files(*.log)|*.*", FileName = $"kryptlog-{DateTime.Now:yyMMdd-HHmm}" };
            var dialogResult = saveFileDialog.ShowDialog();
            if (dialogResult != DialogResult.OK || string.IsNullOrWhiteSpace(saveFileDialog.FileName)) return;
            using (var fStream = saveFileDialog.OpenFile())
            {
                var logHeader = Utils.GenerateLogHeader();
                fStream.Write(logHeader, 0, logHeader.Length);
                fStream.Write(Utils.GetBytes(txtStatus.Text), 0, txtStatus.Text.Length);
            }
            _status.WriteLine("Exported log to " + saveFileDialog.FileName);
        }
    }
}
