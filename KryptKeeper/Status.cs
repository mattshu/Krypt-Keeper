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
            statusBox.AppendText(timestamp);
            statusBox.AppendText(msg + "...");
        }

        private void finishPending()
        {
            statusBox.AppendText("done! " + pendingTimeEnd() + newLine);
        }

        private string pendingTimeEnd()
        {
            var time = TimeSpan.FromTicks(DateTime.Now.Ticks - pendingStartTime.Ticks);
            var sb = new StringBuilder();
            if (time.Days > 0) // Gods forbid
                sb.Append(time.Days + "d ");
            if (time.Hours > 0)
                sb.Append(time.Hours + "h ");
            if (time.Minutes > 0)
                sb.Append(time.Minutes + "m ");
            if (time.Seconds > 0)
                sb.Append(time.Seconds + "s ");
            if (time.Milliseconds > 0)
                sb.Append(time.Milliseconds + "ms");
            return "(" + sb + ")";
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
            statusBox.AppendText(timestamp);
            statusBox.AppendText(msg + newLine);
        }

        private static string timestamp => "[" + DateTime.Now.ToString("HH:mm:ss.fff") + "]: ";
    }
}