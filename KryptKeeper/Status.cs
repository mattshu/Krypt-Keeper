using System;
using MetroFramework.Controls;

namespace KryptKeeper
{
    public class Status
    {
        private static Status _instance;
        private readonly MainWindow _mainWindow;
        private readonly string newLine = Environment.NewLine;
        private readonly MetroLabel _lblOperation;
        private readonly MetroLabel _lblProcessingFile;
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
            _statusBox = (MetroTextBox) statusObjs[2];
        }


        public static Status GetInstance()
        {
            return _instance ?? throw new Exception(@"Unable to get _instance of status window!");
        }

        public void UpdateOperationLabel(string msg)
        {
            _mainWindow.Invoke((Action) (() => _lblOperation.Text = msg));
        }

        public void UpdateProcessingLabel(string msg)
        {
            _mainWindow.Invoke((Action) (() => _lblProcessingFile.Text = msg));
        }

        public void WriteLine(string msg)
        {
            if (_isPending)
                finishPending();
            _isPending = false;
            _mainWindow.Invoke((Action) (() => _statusBox.AppendText(timestamp + msg + newLine)));
        }

        public void WritePending(string msg)
        {
            if (_isPending)
                finishPending();
            else
                _isPending = true;
            _pendingStartTime = DateTime.Now;
            _mainWindow.Invoke((Action) (() => _statusBox.AppendText(timestamp + msg + "...")));
        }

        private static string timestamp => $"[{DateTime.Now:HH:mm:ss.fff}]: ";

        private void finishPending()
        {
            _mainWindow.Invoke((Action) (() =>
                _statusBox.AppendText($"done! ({Helper.GetSpannedTime(_pendingStartTime.Ticks)}){newLine}")));
        }
    }
}