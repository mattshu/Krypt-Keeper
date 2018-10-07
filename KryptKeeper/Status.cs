using MetroFramework.Controls;
using System;
using System.Windows.Forms;

namespace KryptKeeper {
    public partial class Status
    {
        private static Status _Instance;
        private static string Timestamp => $"[{DateTime.Now:HH:mm:ss.fff}]: ";
        private static ProcessRates _ProcessRates;
        private readonly MetroLabel _lblFileWorked;
        private readonly MetroLabel _lblOperation;
        private readonly MetroLabel _lblProcessingRate;
        private readonly MetroLabel _lblTimeElapsed;
        private readonly MetroLabel _lblTimeRemaining;
        private readonly MainWindow _mainWindow;
        private readonly string _newLine = Environment.NewLine;
        private readonly MetroTextBox _txtLogBox;
        private bool _isPending;
        private DateTime _pendingStartTime;

        public Status(MainWindow mainWindow)
        {
            if (_Instance != null)
                return;
            _Instance = this;
            _mainWindow = mainWindow;
            _lblOperation = mainWindow.GetOperationText;
            _lblFileWorked = mainWindow.GetFileWorkedText;
            _lblProcessingRate = mainWindow.GetProcessingRateText;
            _txtLogBox = mainWindow.GetLogBox;
            _lblTimeElapsed = mainWindow.GetTimeElapsedText;
            _lblTimeRemaining = mainWindow.GetTimeRemainingText;
        }

        public static Status GetInstance()
        {
            return _Instance ?? throw new Exception(@"Unable to get instance of status window!");
        }

        public void StartCollection(CipherOptions options)
        {
            _ProcessRates = new ProcessRates(this);
            _ProcessRates.Start();
        }

        public void StopCollection()
        {
            _ProcessRates.Stop();
            SetOperationText("Done!");
            SetFileWorkedText("");
            SetProcessingRateText("");
            SetTimeRemaining("");
        }

        public void SetOperationText(string msg)
        {
            updateLabel(_lblOperation, msg);
        }

        public void SetFileWorkedText(string msg)
        {
            updateLabel(_lblFileWorked, msg);
            _mainWindow.SetLastFileWorked(msg);
        }

        public void SetProcessingRateText(string msg)
        {
            updateLabel(_lblProcessingRate, msg);
        }

        public void SetTimeRemaining(string msg)
        {
            updateLabel(_lblTimeRemaining, msg);
        }

        public void SetTimeElapsed(string msg)
        {
            updateLabel(_lblTimeElapsed, msg);
        }

        private void updateLabel(Control label, string msg) {
            _mainWindow?.Invoke((Action)(() => label.Text = msg));
        }

        public void WriteLine(string msg)
        {
            if (_isPending)
                finishPending();
            _isPending = false;
            updateStatus(Timestamp + msg + _newLine);
        }

        private void finishPending() {
            updateStatus($"done! ({Utils.GetSpannedTime(_pendingStartTime.Ticks)}){_newLine}");
        }

        private void updateStatus(string msg) {
            _mainWindow?.Invoke((Action)(() => _txtLogBox?.AppendText(msg)));
        }

        public void WritePending(string msg)
        {
            if (_isPending)
                finishPending();
            else
                _isPending = true;
            _pendingStartTime = DateTime.Now;
            updateStatus(Timestamp + msg + "...");
        }
    }
}