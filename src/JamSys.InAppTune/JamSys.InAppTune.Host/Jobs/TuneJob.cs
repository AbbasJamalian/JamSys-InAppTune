using JamSys.InAppTune.Agent;

namespace JamSys.InAppTune.Host.Jobs
{
    public class TuneJob : Job
    {
        public TuneJob()
        {
            Title = "Tune Agent";
        }

        protected override void PreStart()
        {
            base.PreStart();
        }

        protected override bool DoWork()
        {
            bool continueWork = true;
            try
            {
                TuningAgent.Instance.ManualTune();
            }
            catch (Exception ex)
            {
                this.Message = ex.Message;
                continueWork = false;
            }

            return continueWork;
        }
    }
}
