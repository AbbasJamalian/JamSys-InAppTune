using JamSys.InAppTune.Agent;

namespace JamSys.InAppTune.Rewards
{
    public class ThroughputRewardFunction : IRewardFunction
    {
        public float CalculateReward(PerformanceMetrics metrics)
        {
            float totalReward = 0;

            float baselineDelta = GetBaselineDelta(metrics);
            float currentDelta = GetCurrentDelta(metrics);

            if (baselineDelta > 0)
            {
                totalReward = (float)((Math.Pow(baselineDelta + 1f, 2) - 1f) * Math.Abs(1f + currentDelta));
            }
            else
            {
                totalReward = (float)((-1f * (Math.Pow(1f - baselineDelta, 2) - 1f)) * Math.Abs(1f - currentDelta));
            }

            if (totalReward > 0 && currentDelta < 0)
                totalReward = 0;

            // Clipping
            if(totalReward > 2)
                totalReward = 2;
            else if (totalReward < -2)
                totalReward = -2;

            return totalReward;
        }

        float GetBaselineDelta(PerformanceMetrics metrics)
        {
            float result = 0;

            result = (metrics.ThroughputAfterAction - metrics.BaselineThroughput) / metrics.BaselineThroughput;

            return result;
        }

        float GetCurrentDelta(PerformanceMetrics metrics)
        {
            float result = 0;

            result = (metrics.ThroughputAfterAction - metrics.ThroughputBeforeAction) / metrics.ThroughputBeforeAction;

            return result;
        }

    }
}