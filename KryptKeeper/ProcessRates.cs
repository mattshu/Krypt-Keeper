using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;

namespace KryptKeeper {
    public partial class Status
    {
        private class ProcessRates
        {
            private const int MAX_DATA_POINTS = 10;
            private const int INTERVAL = 1000;
            private static List<long> _data;
            private readonly Timer _timer;
            private readonly Status _status;
            private long _totalDataSize;
            private string _processingRate;

            public ProcessRates(Status status)
            {
                _status = status;
                _timer = new Timer { Enabled = true, Interval = INTERVAL };
                _timer.Elapsed += timerElapsed;
            }

            public void Start()
            {
                _data = new List<long>();
                _totalDataSize = Cipher.GetTotalSize();
                _timer.Start();
            }

            public void Stop()
            {
                _timer.Stop();
                _data = new List<long>();
                _totalDataSize = 0;
            }

            private void timerElapsed(object sender, ElapsedEventArgs e)
            {
                _status.UpdateTimeElapsed(Cipher.GetElapsedTime(hideMs: true) + @"elapsed");
                var elapsed = Cipher.GetElapsedBytes();
                if (elapsed <= 0)
                    return;
                addDataPoint(elapsed);
                _processingRate = ((long) _data.Average(x => x * (1000 / INTERVAL))).BytesToSizeString();
                _status.UpdateProcessingRate($"Processing speed: {_processingRate}/s");
                _status.UpdateTimeRemaining(calculateTimeRemaining());
            }

            private static void addDataPoint(long data) {
                _data.Insert(0, data);
                _data = _data.GetRange(0, Math.Min(_data.Count, MAX_DATA_POINTS));
            }

            private string calculateTimeRemaining()
            {
                var total = _totalDataSize;
                var rate = Utils.SizeToBytesString(_processingRate);
            }
        }
    }
}