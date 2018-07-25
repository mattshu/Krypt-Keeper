using System;
using System.Text;
using System.Windows.Forms;

namespace KryptKeeper
{
    public class Status
    {
        private static Status instance;
        private readonly TextBox statusBox;
        private readonly ProgressBar progressBar;
        private readonly string newLine = Environment.NewLine;
        private bool isPending;
        private DateTime pendingStartTime;

        public Status(TextBox statusBox, ProgressBar progressBar)
        {
            if (instance != null) return;
            instance = this;
            this.statusBox = statusBox;
            this.progressBar = progressBar;
        }

        public static Status GetInstance()
        {
            return instance;
        }

        public void UpdateProgress(int progress, int limit)
        {
            progressBar.Invoke((Action) delegate
            {
                progressBar.Maximum = limit;
                progressBar.Step = 1;
                progressBar.Value = progress;
            });
        }

        public void WritePending(string msg)
        {
            if (isPending)
                finishPending();
            else
                isPending = true;
            pendingStartTime = DateTime.Now;
            statusBox.Invoke((Action) delegate
            {
                statusBox.AppendText(Timestamp + msg + "...");
            });
        }

        private void finishPending()
        {
            statusBox.Invoke((Action) delegate
            {
                statusBox.AppendText("done! " + Helper.GetSpannedTime(pendingStartTime.Ticks) + newLine);
            });
        }

        public void PendingComplete()
        {
            if (!isPending) return;
            isPending = false;
            finishPending();
        }

        public void WriteLine(string msg)
        {
            if (isPending)
                finishPending();
            isPending = false;
            statusBox.Invoke((Action) delegate
            {
                statusBox.AppendText(Timestamp + msg + newLine);
            });
        }

        private static string Timestamp => "[" + DateTime.Now.ToString("HH:mm:ss.fff") + "]: ";
    }
}