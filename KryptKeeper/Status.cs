using System;
using System.Text;
using System.Windows.Forms;

namespace KryptKeeper
{
    public class Status
    {
        private static Status instance;
        private readonly TextBox statusBox;
        private readonly string newLine = Environment.NewLine;
        private bool isPending;
        private DateTime pendingStartTime;

        public Status(TextBox statusBox)
        {
            if (instance != null) return;
            instance = this;
            this.statusBox = statusBox;
        }

        public static Status GetInstance()
        {
            return instance;
        }

        public void WritePending(string msg)
        {
            if (isPending)
                finishPending();
            else
                isPending = true;
            pendingStartTime = DateTime.Now;
            statusBox.AppendText(Timestamp);
            statusBox.AppendText(msg + "...");
        }

        private void finishPending()
        {
            statusBox.AppendText("done! " + Helper.GetSpannedTime(pendingStartTime.Ticks) + newLine);
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
            statusBox.AppendText(Timestamp);
            statusBox.AppendText(msg + newLine);
        }

        private static string Timestamp => "[" + DateTime.Now.ToString("HH:mm:ss.fff") + "]: ";
    }
}