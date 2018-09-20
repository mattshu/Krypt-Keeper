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
            private const int INTERVAL = 1000;
            private static List<long> _data;
            private readonly Timer _timer;
            private readonly Status _status;
            private long _totalDataSize;
            private string _processingRate;
            private DateTime _startTime;
            private Stopwatch _stopWatch;

            public ProcessRates(Status status)
            {
                _status = status;
                _stopWatch = new Stopwatch();
                _timer = new Timer { Enabled = true, Interval = INTERVAL };
                _timer.Elapsed += timerElapsed;
            }

            public void Start()
            {
                _startTime = DateTime.Now;
                _stopWatch.Start();
                _data = new List<long>();
                _totalDataSize = Cipher.GetTotalSize();
                _timer.Start();
            }

            public void Stop()
            {
                _timer.Stop();
                _stopWatch.Stop();
                _data = new List<long>();
                _totalDataSize = 0;
                _startTime = new DateTime();
            }

            private void timerElapsed(object sender, ElapsedEventArgs e)
            {
                _status.UpdateTimeElapsed(Utils.GetSpannedTime(_startTime.Ticks, hideMs: true) +  @"elapsed");
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
                //    Console.WriteLine("{0} time remaining",sw.GetEta(x,y).ToString());

                return "Est. time remaining: " + GetEta(_stopWatch, (int)(Cipher.GetElapsedBytes() / 1000), (int)(_totalDataSize / 1000)) + "s"; // TODO this is not right
            }

            private TimeSpan GetEta(Stopwatch sw, int counter, int counterGoal)
            {
                /* this is based off of:
                 * (TimeTaken / linesProcessed) * linesLeft=timeLeft
                 * so we have
                 * (10/100) * 200 = 20 Seconds now 10 seconds go past
                 * (20/100) * 200 = 40 Seconds left now 10 more seconds and we process 100 more lines
                 * (30/200) * 100 = 15 Seconds and now we all see why the copy file dialog jumps from 3 hours to 30 minutes :-)
                 * 
                 * pulled from http://stackoverflow.com/questions/473355/calculate-time-remaining/473369#473369
                 */
                if (counter == 0) return TimeSpan.Zero;
                float elapsedMin = ((float)sw.ElapsedMilliseconds / 1000) / 60;
                float minLeft = (elapsedMin / counter) * (counterGoal - counter); //see comment a
                TimeSpan ret = TimeSpan.FromMinutes(minLeft);
                return ret;
            }
        }
    }
}