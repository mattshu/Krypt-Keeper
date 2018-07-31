using System;
using System.Windows.Forms;

namespace KryptKeeper
{
    public class Status
    {
        private static Status instance;
        private readonly string newLine = Environment.NewLine;
        private readonly ProgressBar progressBar;
        private readonly TextBox statusBox;
        private bool isPending;
        private DateTime pendingStartTime;

        public Status(TextBox statusBox, ProgressBar progressBar)
        {
            if (instance != null) return;
            instance = this;
            this.statusBox = statusBox;
            this.progressBar = progressBar;
        }

        private static string Timestamp => "[" + DateTime.Now.ToString("HH:mm:ss.fff") + "]: ";

        public static Status GetInstance()
        {
            return instance;
        }

        public void PendingComplete()
        {
            if (!isPending) return;
            isPending = false;
            finishPending();
        }

        public void UpdateProgress(int progress, int limit)
        {
            progressBar.Invoke((Action)delegate
           {
               progressBar.Maximum = limit;
               progressBar.Step = 5;
               progressBar.Value = progress;
           });
        }

        public void WriteLine(string msg)
        {
            if (isPending)
                finishPending();
            isPending = false;
            statusBox.Invoke((Action)delegate
           {
               statusBox.AppendText(Timestamp + msg + newLine);
           });
        }

        public void WritePending(string msg)
        {
            if (isPending)
                finishPending();
            else
                isPending = true;
            pendingStartTime = DateTime.Now;
            statusBox.Invoke((Action)delegate
           {
               statusBox.AppendText(Timestamp + msg + "...");
           });
        }

        private void finishPending()
        {
            statusBox.Invoke((Action)delegate
           {
               statusBox.AppendText("done! " + Helper.GetSpannedTime(pendingStartTime.Ticks) + newLine);
           });
        }
    }
}