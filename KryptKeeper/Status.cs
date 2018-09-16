using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Timers;

namespace KryptKeeper
{
    public class Status
    {
        private static Status _instance;
        private static string _timestamp => $"[{DateTime.Now:HH:mm:ss.fff}]: ";
        private static ProcessRate _processRate;
        private readonly MetroLabel _lblFileBeingProcessed;
        private readonly MetroLabel _lblOperationStatus;
        private readonly MetroLabel _lblProcessingRates;
        private readonly MainWindow _mainWindow;
        private readonly string _newLine = Environment.NewLine;
        private readonly MetroTextBox _txtStatus;
        private bool _isPending;
        private DateTime _pendingStartTime;

        private class ProcessRate
        {
            private const int MAX_DATA_POINTS = 25;
            private const int INTERVAL = 500;
            private static List<long> _data;
            private static Timer _timer;
            private static Status _status;

            public ProcessRate(Status status)
            {
                _status = status;
                _data = new List<long>();
                _timer = new Timer { Enabled = true, Interval = INTERVAL };
                _timer.Elapsed += timer_Elapsed;
            }

            public void Start()
            {
                _timer.Start();
            }

            public void Stop()
            {
                _timer.Stop();
                _data = new List<long>();
            }

            private static void timer_Elapsed(object sender, ElapsedEventArgs e)
            {
                add(Cipher.GetElapsedBytes());
                var msg = $"Processing speed: {((long)_data.Average(x => x * (1000 / INTERVAL))).BytesToSizeString()}/s";
                _status.UpdateProcessingRate(msg);
            }

            private static void add(long data) {
                _data.Insert(0, data);
                _data = _data.GetRange(0, Math.Min(_data.Count, MAX_DATA_POINTS));
            }
        }

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
        }

        public static Status GetInstance()
        {
            return _instance ?? throw new Exception(@"Unable to get instance of status window!");
        }

        public void StartProcessRateCollection()
        {
            _processRate = new ProcessRate(this);
            _processRate.Start();
        }

        public void StopProcessRateCollection()
        {
            _processRate.Stop();
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