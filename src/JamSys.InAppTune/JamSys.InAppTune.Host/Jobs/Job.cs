using System;
using System.Diagnostics;
using System.Reactive.Concurrency;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace JamSys.InAppTune.Host.Jobs
{

    public class Job : IJob
    {
        private IDisposable _observable;

        public long Min { get; private set; }
        public long Max { get; private set; }

        public long Iteration { get; private set; } = 0;

        public long CurrentElapsed { get; set; }

        public Subject<(long iteration, long duration)> Stats { get; private set; } = new Subject<(long iteration, long duration)>();

        public string Title { get; set; }

        public string Message { get; set; }

        private long _coreDuration;

        public void Start(int interval)
        {
            if (_observable == null)
            {
                PreStart();
                _observable = Observable.Interval(TimeSpan.FromMilliseconds(interval))
                    .Throttle(TimeSpan.FromMilliseconds(interval))
                    .Subscribe(_ => BackgroundExecute());
            }
        }

        public bool IsRunning => _observable != null;

        public int NumParallelExecutions { get; set; } = 1;

        public int JobParameter { get; set; }

        public int JobLimit { get; set; }

        public void Stop()
        {
            if (_observable != null)
            {
                _observable.Dispose();
                _observable = null;
                GC.Collect();
                PostStop();
            }
        }

        public void Toggle(int interval)
        {
            if (IsRunning)
                Stop();
            else
                Start(interval);

        }

        public void Dispose()
        {
            Stop();

            Stats.Dispose();
        }

        private void BackgroundExecute()
        {
            try
            {
                Stopwatch watch = new Stopwatch();
                watch.Start();
                var continueWork = DoWork();
                watch.Stop();

                long elapsed = _coreDuration >= 0 ? _coreDuration : watch.ElapsedMilliseconds;

                if (Iteration == 0)
                {
                    Min = elapsed;
                    Max = elapsed;
                }
                else if (Iteration > 0)
                {
                    Min = elapsed < Min ? elapsed : Min;
                    Max = elapsed > Max ? elapsed : Max;
                }
                CurrentElapsed = elapsed;

                Iteration++;

                if (continueWork)
                {
                    Stats.OnNext((Iteration, watch.ElapsedMilliseconds));
                }
                else
                {
                    Stats.OnCompleted();
                    Stop();
                }
            }
            catch (Exception ex)
            {
                Stats.OnError(ex);
                Stop();
            }
        }

        protected void SetCoreDuration(long duration)
        { 
            _coreDuration = duration;
        }
        protected virtual bool DoWork()
        {
            return true;
        }

        protected virtual void PreStart()
        {

        }

        protected virtual void PostStop()
        {

        }
    }
}
