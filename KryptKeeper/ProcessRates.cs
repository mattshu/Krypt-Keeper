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
            private static List<long> _ProcessData;
            private readonly Timer _timer;
            private readonly Status _status;
            private readonly Stopwatch _stopWatch;
            private long _totalDataSize;
            private DateTime _startTime;

            public ProcessRates(Status status)
            {
                _status = status;
                _stopWatch = new Stopwatch();
                _timer = new Timer { Enabled = true, Interval = INTERVAL };
                _timer.Elapsed += timerElapsed;
                _totalDataSize = Cipher.GetTotalSize();
            }

            public void Start()
            {
                _stopWatch.Start();
                _timer.Start();
                _startTime = DateTime.Now;
                _ProcessData = new List<long>();
            }

            public void Stop()
            {
                _timer.Stop();
                _stopWatch.Stop();
                _stopWatch.Reset();
                _ProcessData.Clear();
                _totalDataSize = 0;
            }

            private void timerElapsed(object sender, ElapsedEventArgs e)
            {
                _status.UpdateTimeElapsed(Utils.GetSpannedTime(_startTime.Ticks, hideMs: true) +  @"elapsed");
                var elapsed = Cipher.GetElapsedBytes();
                if (elapsed <= 0)
                    return;
                _ProcessData.InsertAndTrim(elapsed, MAX_DATA_POINTS);
                var averageProcessRate = ((long) _ProcessData.Average(x => x * (1000 / INTERVAL))).BytesToSizeString();
                _status.UpdateProcessRate($"Processing speed: {averageProcessRate}/s");
                _status.UpdateTimeRemaining($"Est. time remaining: {TimeSpan.FromSeconds(Math.Floor(getSecondsRemaining()))}");
            }

            private double getSecondsRemaining()
            {
                var payloadState = Cipher.GetPayloadState();
                if (payloadState <= 0) return 0;
                var remaining = (_stopWatch.Elapsed.Seconds / (double) payloadState) * (_totalDataSize - payloadState);
                Console.WriteLine($@"({_stopWatch.Elapsed.Seconds}s / {(double)payloadState}B) * ({_totalDataSize}B - {payloadState}B) = {remaining} seconds remain");
                return remaining;
            }
        }
    }
}