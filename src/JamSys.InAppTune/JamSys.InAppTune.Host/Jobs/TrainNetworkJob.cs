using JamSys.InAppTune.Agent;

namespace JamSys.InAppTune.Host.Jobs
{
    public class TrainNetworkJob : Job
    {
        public TrainNetworkJob()
        {
            Title = "Train DeepQNetwork";
        }

        protected override bool DoWork()
        {
            bool continueWork = true;
            try
            {
                float loss = TuningAgent.Instance.Train();

                //loss = Math.Abs(loss);
                //SetCoreDuration(Convert.ToInt64(loss));
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
