using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Timers;

namespace KryptKeeper {
    public partial class Status
    {
        private class ProcessRates
        {
            private const int MAX_DATA_POINTS = 5;
            private const int INTERVAL = 500;
            private readonly Status _status;
            private readonly Timer _timer;
            private readonly Stopwatch _stopWatch;
            private static List<long> _DataProcessed;
            private long _totalDataLength;

            public ProcessRates(Status status)
            {
                _status = status;
                _timer = new Timer { Enabled = true, Interval = INTERVAL };
                _timer.Elapsed += timerElapsed;
                _stopWatch = new Stopwatch();
                _DataProcessed = new List<long>();
            }

            public void Start()
            {
                _totalDataLength = Cipher.GetTotalSize();
                _stopWatch.Start();
                _timer.Start();
            }

            public void Stop()
            {
                _timer.Stop();
                _stopWatch.Reset();
                _DataProcessed.Clear();
                _totalDataLength = 0;
            }

            private void timerElapsed(object sender, ElapsedEventArgs e)
            {
                _status.SetTimeElapsed(Utils.GetSpannedTime(DateTime.Now.Ticks - _stopWatch.ElapsedTicks, hideMs: true) +  @"elapsed");
                _status.SetTimeRemaining($"Est. time remaining: {getTimeRemaining()}");
                var elapsedBytes = Cipher.GetElapsedBytes();
                if (elapsedBytes <= 0) return;
                _DataProcessed.InsertAndTrim(elapsedBytes, MAX_DATA_POINTS);
                if (_DataProcessed.Count <= 0) return;
                var averageProcessRate = ((long) _DataProcessed.Average(x => x * (1000 / INTERVAL))).BytesToSizeString();
                _status.SetProcessingRateText($"Processing speed: {averageProcessRate}/s");
            }

            // TODO Could be more accurate, especially near the end
            private string getTimeRemaining()
            {
                var payloadState = Cipher.GetPayloadState();
                if (payloadState <= 0) return "";
                var remaining = (_stopWatch.Elapsed.Seconds / (double) payloadState) * (_totalDataLength - payloadState);
                return TimeSpan.FromSeconds(Math.Ceiling(remaining)).ToString();
            }
        }
    }
}