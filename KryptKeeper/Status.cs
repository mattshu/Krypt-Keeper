using System;
using System.Windows.Forms;

namespace KryptKeeper
{
    public class Status
    {
        private static Status instance;
        private readonly TextBox statusBox;
        private readonly string newLine = Environment.NewLine;
        private bool IsPending;

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
            if (IsPending)
                statusBox.AppendText("done!" + newLine);
            else
                IsPending = true;
            statusBox.AppendText(Timestamp);
            statusBox.AppendText(msg + "...");
        }

        public void PendingComplete()
        {
            if (!IsPending) return;
            IsPending = false;
            statusBox.AppendText("done!" + newLine);
        }

        public void WriteLine(string msg)
        {
            if (IsPending)
                statusBox.AppendText("done!" + newLine);
            IsPending = false;
            statusBox.AppendText(Timestamp);
            statusBox.AppendText(msg + newLine);
        }

        private static string Timestamp => "[" + DateTime.Now.ToString("HH:mm:ss.fff") + "]: ";
    }
}