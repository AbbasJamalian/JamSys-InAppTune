using JamSys.InAppTune.Agent;

namespace JamSys.InAppTune.Rewards
{
    public class CompositeRewardFunction : IRewardFunction
    {
        public float CalculateReward(PerformanceMetrics metrics)
        {
            float totalReward = 0;

            var latencyReward = new LatencyRewardFunction();
            var throughputReward = new ThroughputRewardFunction();

            totalReward = (0.6f * latencyReward.CalculateReward(metrics)) + (0.4f * throughputReward.CalculateReward(metrics));

            return totalReward;
        }

    }
}