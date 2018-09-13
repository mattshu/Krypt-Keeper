using MetroFramework.Controls;
using System;

namespace KryptKeeper
{
    public class Status
    {

        private static Status _instance;
        private readonly MainWindow _mainWindow;
        private readonly string newLine = Environment.NewLine;
        private readonly MetroLabel _lblOperation;
        private readonly MetroLabel _lblProcessingFile;
        private readonly MetroLabel _lblProcessingRates;
        private readonly MetroTextBox _statusBox;
        private bool _isPending;
        private DateTime _pendingStartTime;

        public Status(MainWindow mainWindow)
        {
            if (_instance != null) return;
            _instance = this;
            _mainWindow = mainWindow;
            var statusObjs = mainWindow.GetStatusObjects();
            if (statusObjs.Count <= 0)
                throw new Exception(@"Unable to get status objects!");
            _lblOperation = (MetroLabel) statusObjs[0];
            _lblProcessingFile = (MetroLabel) statusObjs[1];
            _lblProcessingRates = (MetroLabel) statusObjs[2];
            _statusBox = (MetroTextBox) statusObjs[3];
        }


        public static Status GetInstance()
        {
            return _instance ?? throw new Exception(@"Unable to get _instance of status window!");
        }

        public void UpdateOperationLabel(string msg)
        {
            _mainWindow.Invoke((Action)(() => _lblOperation.Text = msg));
        }

        public void UpdateProcessingLabel(string msg)
        {
            _mainWindow.Invoke((Action)(() => _lblProcessingFile.Text = msg));
        }

        public void UpdateRatesLabel(string processRate)
        {
            _mainWindow.Invoke((Action)(() => _lblProcessingRates.Text = processRate));
        }

        public void WriteLine(string msg, long fileSize = 0)
        {
            if (_isPending)
                finishPending(fileSize);
            _isPending = false;
            _mainWindow.Invoke((Action)(() => _statusBox.AppendText(_timestamp + msg + newLine)));
        }

        public void WritePending(string msg, long fileSize = 0)
        {
            if (_isPending)
                finishPending(fileSize);
            else
                _isPending = true;
            _pendingStartTime = DateTime.Now;
            _mainWindow.Invoke((Action)(() => _statusBox.AppendText(_timestamp + msg + "...")));
        }

        private static string _timestamp => $"[{DateTime.Now:HH:mm:ss.fff}]: ";

        private void finishPending(long fileSize = 0)
        {
            if (fileSize > 0)
                UpdateRatesLabel($@"{fileSize / _pendingStartTime.Second} bps");
            _mainWindow.Invoke((Action)(() =>
               _statusBox.AppendText($"done! ({Utils.GetSpannedTime(_pendingStartTime.Ticks)}){newLine}")));
        }


    }
}