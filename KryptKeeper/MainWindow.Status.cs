using System;
using System.Windows.Forms;

namespace KryptKeeper
{
    public partial class MainWindow
    {
        public class Status
        {
            private readonly TextBox _statusBox;
            public Status(TextBox statusBox)
            {
                _statusBox = statusBox;
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