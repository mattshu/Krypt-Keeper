using MetroFramework.Controls;
using System;
using System.Linq;

namespace KryptKeeper {
    public partial class Status
    {
        private static Status _instance;
        private static string _timestamp => $"[{DateTime.Now:HH:mm:ss.fff}]: ";
        private static ProcessRates _processRates;
        private readonly MetroLabel _lblFileBeingProcessed;
        private readonly MetroLabel _lblOperationStatus;
        private readonly MetroLabel _lblProcessingRates;
        private readonly MetroLabel _lblTimeElapsed;
        private readonly MetroLabel _lblTimeRemaining;
        private readonly MainWindow _mainWindow;
        private readonly string _newLine = Environment.NewLine;
        private readonly MetroTextBox _txtStatus;
        private bool _isPending;
        private DateTime _pendingStartTime;

        public Status(MainWindow mainWindow)
        {
            if (_instance != null)
                return;
            _instance = this;
            _mainWindow = mainWindow;
            var statusObjs = mainWindow.GetStatusObjects();
            if (statusObjs.Count <= 0)
                throw new Exception(@"Unable to get status objects!");
            _lblOperationStatus = (MetroLabel) statusObjs[0];
            _lblFileBeingProcessed = (MetroLabel) statusObjs[1];
            _lblProcessingRates = (MetroLabel) statusObjs[2];
            _txtStatus = (MetroTextBox) statusObjs[3];
            _lblTimeElapsed = (MetroLabel) statusObjs[4];
            _lblTimeRemaining = (MetroLabel) statusObjs[5];
        }

        public static Status GetInstance()
        {
            return _instance ?? throw new Exception(@"Unable to get instance of status window!");
        }

        public void StartProcessRateCollection()
        {
            _processRates = new ProcessRates(this);
            _processRates.Start();
        }

        public void StopProcessRateCollection()
        {
            _processRates.Stop();
        }

        public void SetFileOperationMsg(string msg)
        {
            updateLabel(_lblOperationStatus, msg);
        }

        public void SetFileProcessingMsg(string msg)
        {
            updateLabel(_lblFileBeingProcessed, msg);
        }

        public void UpdateProcessingRate(string msg)
        {
            updateLabel(_lblProcessingRates, msg);
        }

        public void UpdateTimeElapsed(string msg)
        {
            updateLabel(_lblTimeElapsed, msg);
        }

        public void UpdateTimeRemaining(string msg)
        {
            updateLabel(_lblTimeRemaining, msg);
        }

        public void WriteLine(string msg)
        {
            if (_isPending)
                finishPending();
            _isPending = false;
            updateStatus(_timestamp + msg + _newLine);
        }

        public void WritePending(string msg)
        {
            if (_isPending)
                finishPending();
            else
                _isPending = true;
            _pendingStartTime = DateTime.Now;
            updateStatus(_timestamp + msg + "...");
        }

        private void updateLabel(MetroLabel label, string msg)
        {
            _mainWindow?.Invoke((Action)(() => label.Text = msg));
        }

        private void finishPending() 
        {
            updateStatus($"done! ({Utils.GetSpannedTime(_pendingStartTime.Ticks)}){_newLine}");
        }

        private void updateStatus(string msg)
        {
            _mainWindow?.Invoke((Action)(() => _txtStatus?.AppendText(msg)));
        }
    }
}