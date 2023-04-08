using JamSys.InAppTune.Host.Jobs;

namespace JamSys.InAppTune.Host.Components
{
    public class Store
    {
        public IJob SelectedJob { get; set; }

        public int NumParallelExecutions { get; set; } = 4;


        public int JobParameter { get; set; } = 7;

        public int JobLimit { get; set; } = 20000;

    }
}
