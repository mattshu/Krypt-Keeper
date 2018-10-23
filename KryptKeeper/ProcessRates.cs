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
            private const int MAX_DATA_POINTS = 10;
            private const int INTERVAL = 500;
            private readonly Status _status;
            private readonly Timer _timer;
            private readonly Stopwatch _stopWatch;
            private static List<long> _DataProcessed;
            private DateTime _startTime;
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
                _startTime = DateTime.Now;
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
                try
                {
                    _status.SetTimeElapsed(Utils.GetSpannedTime(_startTime.Ticks, hideMs: true) + @"elapsed");
                    var secondsRemaining = (int) Math.Floor(getSecondsRemaining());
                    if (secondsRemaining <= -1) return;
                    _status.SetTimeRemaining($"Est. time remaining: {TimeSpan.FromSeconds(secondsRemaining)}");
                    var elapsedBytes = Cipher.GetElapsedBytes();
                    if (elapsedBytes <= 0) return;
                    _DataProcessed.Push(elapsedBytes, MAX_DATA_POINTS);
                    if (_DataProcessed.Count <= 0) return;
                    var averageProcessRate =
                        ((long) _DataProcessed.Average(x => x * (1000 / INTERVAL))).BytesToSizeString();
                    _status.SetProcessingRateText($"Processing speed: {averageProcessRate}/s");
                }
                catch (InvalidOperationException)
                {
                    // Ignore collection modified errors
                }
            }

            private long _lastPayloadState;
            private double getSecondsRemaining()
            {
                var payloadState = Cipher.GetPayloadState();
                if (payloadState <= 0 || payloadState == _lastPayloadState) return -1;
                _lastPayloadState = payloadState;
                return _stopWatch.Elapsed.TotalSeconds / payloadState * (_totalDataLength - payloadState);
            }
        }
    }
}