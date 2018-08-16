using System;
using System.Windows.Forms;

namespace KryptKeeper
{
    public class Status
    {
        private static Status _instance;
        private readonly string newLine = Environment.NewLine;
        private readonly CustomProgressBar _progressCurrent;
        private readonly CustomProgressBar _progressTotal;
        private readonly TextBox _statusBox;
        private bool _isPending;
        private DateTime _pendingStartTime;

        public Status(TextBox statusBox, CustomProgressBar progressCurrent, CustomProgressBar progressTotal)
        {
            if (_instance != null) return;
            _instance = this;
            _statusBox = statusBox;
            _progressCurrent = progressCurrent;
            _progressTotal = progressTotal;
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

        public void UpdateProgress(int progressCurrent, int progressTotal)
        {
            _progressCurrent.Invoke((Action)delegate
            {
                _progressCurrent.Maximum = 100;
                _progressCurrent.Step = 1;
                _progressCurrent.Value = progressCurrent;
            });
            _progressTotal.Invoke((Action)delegate
            {
                _progressTotal.Maximum = 100;
                _progressTotal.Step = 1;
                _progressTotal.Value = progressTotal;
            });
        }

        public void WriteLine(string msg)
        {
            if (_isPending)
                finishPending();
            _isPending = false;
            _statusBox.Invoke((Action)delegate
           {
               _statusBox.AppendText(Timestamp + msg + newLine);
           });
        }

        public void WritePending(string msg)
        {
            if (_isPending)
                finishPending();
            else
                _isPending = true;
            _pendingStartTime = DateTime.Now;
            _statusBox.Invoke((Action)delegate
           {
               _statusBox.AppendText(Timestamp + msg + "...");
           });
        }

        private void finishPending()
        {
            _statusBox.Invoke((Action)delegate
           {
               _statusBox.AppendText("done! " + Helper.GetSpannedTime(_pendingStartTime.Ticks) + newLine);
           });
        }
    }
}