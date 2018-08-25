using System;
using MetroFramework.Controls;

namespace KryptKeeper
{
    public class Status
    {
        private static Status _instance;
        private readonly MainWindow _mainWindow;
        private readonly string newLine = Environment.NewLine;
        private readonly MetroLabel _lblCurrentPercentage;
        private readonly MetroLabel _lblTotalPercentage;
        private readonly MetroLabel _lblOperation;
        private readonly MetroLabel _lblFileBefore;
        private readonly MetroLabel _lblFileAfter;
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
            _lblCurrentPercentage = (MetroLabel) statusObjs[0];
            _lblTotalPercentage = (MetroLabel) statusObjs[1];
            _lblOperation = (MetroLabel) statusObjs[2];
            _lblFileBefore = (MetroLabel) statusObjs[3];
            _lblFileAfter = (MetroLabel) statusObjs[4];
            _statusBox = (MetroTextBox) statusObjs[5];
        }


        public static Status GetInstance()
        {
            return _instance ?? throw new Exception(@"Unable to get _instance of status window!");
        }

        public void UpdateBeforeLabel(string msg)
        {
            _mainWindow.Invoke((Action) (() => _lblFileBefore.Text = msg));
        }

        public void UpdateOperationLabel(string msg)
        {
            _mainWindow.Invoke((Action) (() => _lblOperation.Text = msg));
        }

        public void UpdateAfterLabel(string msg)
        {
            _mainWindow.Invoke((Action) (() => _lblFileAfter.Text = msg));
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

        private static string timestamp => "[" + DateTime.Now.ToString("HH:mm:ss.fff") + "]: ";

        private void finishPending()
        {
            _mainWindow.Invoke((Action) (() =>
                _statusBox.AppendText("done! " + Helper.GetSpannedTime(_pendingStartTime.Ticks) + newLine)));
        }
    }
}