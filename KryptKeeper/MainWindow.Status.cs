using System;
using System.Windows.Forms;

namespace KryptKeeper
{
    public partial class MainWindow
    {
        public class Status
        {
            private readonly TextBox _statusBox;
            public bool IsPending;

            public Status(TextBox statusBox)
            {
                _statusBox = statusBox;
            }

            public void WritePending(string msg)
            {
                if (IsPending)
                    _statusBox.AppendText("done!" + Environment.NewLine);
                else
                    IsPending = true;
                _statusBox.AppendText(msg + "...");
            }

            public void PendingComplete()
            {
                if (!IsPending) return;
                IsPending = false;
                _statusBox.AppendText("done!" + Environment.NewLine);
            }

            public void WriteLine(string msg)
            {
                string timestamp = "[" + DateTime.Now.ToString("HH:mm:ss") + "]: ";
                _statusBox.AppendText(timestamp);
                _statusBox.AppendText(msg + Environment.NewLine);
            }
        }
    }
}