namespace JamSys.InAppTune.Host.Jobs
{
    public interface IJob : IDisposable
    {
        string Title { get; set; }

        string Message { get; set; }

        bool IsRunning { get; }

        int NumParallelExecutions { set; get; }

        public int JobParameter { get; set; }

        public int JobLimit { get; set; }


        void Start(int interval);
        void Stop();
        void Toggle(int interval);
    }
}
