using MetroFramework.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace KryptKeeper
{
    public class Status
    {
        private static Status _instance;
        private static string _timestamp => $"[{DateTime.Now:HH:mm:ss.fff}]: ";
        private static Timer _processSpeedTimer;
        private static List<long> _processSpeedData;
        private readonly MetroLabel _lblFileBeingProcessed;
        private readonly MetroLabel _lblOperationStatus;
        private readonly MetroLabel _lblProcessingRates;
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
            _processSpeedTimer = new Timer {Enabled = true, Interval = 500};
            _processSpeedTimer.Elapsed += _processSpeedTimer_Elapsed;
            _processSpeedData = new List<long>(25); // Collect 25 data points to get average process speed
        }

        public static Status GetInstance()
        {
            return _instance ?? throw new Exception(@"Unable to get instance of status window!");
        }

        public void StartProcessSpeedTimer()
        {
            _processSpeedTimer.Start();
        }

        public void StopProcessSpeedTimer()
        {
            _processSpeedTimer.Stop();
            _processSpeedData.Clear();
        }

        public void SetFileOperationMsg(string msg)
        {
            updateLabel(_lblOperationStatus, msg);
        }

        public void SetFileProcessing(string msg)
        {
            updateLabel(_lblFileBeingProcessed, msg);
        }

        public void UpdateProcessingSpeedMsg(string msg)
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

        private void _processSpeedTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            Console.WriteLine();
            _processSpeedData.Add(Cipher.GetElapsedBytes());
            // x * 2 because data point captured @ 500ms
            var msg = $"Processing speed: {((long)_processSpeedData.Average(x => x * 2)).BytesToSizeString()}/s";
            updateLabel(_lblProcessingRates, msg);
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