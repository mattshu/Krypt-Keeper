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
            if (closeAfterCurrentOperation) return;
            updateProgress((ProgressPacket)e.UserState);
        }

        private void backgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (closeAfterCurrentOperation)
            {
                Close();
                return;
            }
            _status.WriteLine("Operation finished. " + Cipher.GetElapsedTime());
            disableButtonsDuringOperation(false);
            updateProgress();
            lblFilesToBeProcessed.Text = e.Cancelled ? "Some" : "All" + " files processed";
            lblJobInformation.Text = "";
            lblProcessingFile.Text = "";
            lblOperationStatus.Text = @"Done!";
            Cipher.StartProgressTimer(false);
            _fileList.Reset();
            datagridFileList.DataSource = _fileList.GetList();
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

        private void updateProgress(ProgressPacket packet = null)
        {
            if (packet == null)
            {
                lblCurrentPercentage.Text = @"100%";
                lblTotalFilePercentage.Text = @"100%";
                progressCurrent.Value = 100;
                progressTotalBytes.Value = 100;
                progressTotalFiles.Value = 100;
                return;
            }
            var currentFileProgress = packet.GetCurrentFileProgress();
            var totalPayloadProgress = packet.GetTotalPayloadProgress();
            var totalFileProgress = packet.GetTotalFilesProgress();
            progressCurrent.Value = Helper.Clamp(currentFileProgress, 0, 100);
            lblCurrentPercentage.Text = $@"{currentFileProgress}%";
            progressTotalBytes.Value = Helper.Clamp(totalPayloadProgress, 0, 100);
            progressTotalFiles.Value = Helper.Clamp(totalFileProgress, 0, 100);
            lblTotalFilePercentage.Text = $@"{totalFileProgress}%";
            lblFilesToBeProcessed.Text = Cipher.GetFileProgress();
            lblTimeElapsed.Text = Cipher.GetElapsedTime(hideMs: true) + @"elapsed";
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
    }
}
