using System;
using System.Windows.Forms;

namespace KryptKeeper
{
    public class Status
    {
        private static Status _instance;
        private readonly MainWindow _mainWindow;
        private readonly string newLine = Environment.NewLine;
        private readonly CustomProgressBar _progressCurrent;
        private readonly CustomProgressBar _progressTotal;
        private readonly TextBox _statusBox;
        private bool _isPending;
        private DateTime _pendingStartTime;

        public Status(MainWindow mainWindow)
        {
            if (_instance != null) return;
            _instance = this;
            _mainWindow = mainWindow;
            _statusBox = mainWindow.GetStatusBox();
            _progressCurrent = mainWindow.GetProgressBarCurrent();
            _progressTotal = mainWindow.GetProgressBarTotal();
        }

        private static string Timestamp => "[" + DateTime.Now.ToString("HH:mm:ss.fff") + "]: ";

        public static Status GetInstance()
        {
            return _instance ?? throw new Exception(@"Unable to get _instance of status window!");
        }

        public void PendingComplete()
        {
            if (!_isPending) return;
            _isPending = false;
            finishPending();
        }

        public void WriteLine(string msg)
        {
            if (_isPending)
                finishPending();
            _isPending = false;
            _mainWindow.Invoke((Action)(() => _statusBox.AppendText(Timestamp + msg + newLine)));
        }

        public void WritePending(string msg)
        {
            if (_isPending)
                finishPending();
            else
                _isPending = true;
            _pendingStartTime = DateTime.Now;
            _mainWindow.Invoke((Action)(() => _statusBox.AppendText(Timestamp + msg + "...")));
        }

        private void finishPending()
        {
            _mainWindow.Invoke((Action)(() => _statusBox.AppendText("done! " + Helper.GetSpannedTime(_pendingStartTime.Ticks) + newLine)));
        }
    }
}