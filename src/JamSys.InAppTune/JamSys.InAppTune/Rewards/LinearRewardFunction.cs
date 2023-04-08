using JamSys.InAppTune.Agent;

namespace JamSys.InAppTune.Rewards
{
    public class LinearRewardFunction : IRewardFunction
    {
        public float CalculateReward(PerformanceMetrics metrics)
        {
            float totalReward = 0;

            float currentDelta = GetCurrentDelta(metrics);
            float baselineDelta = GetBaselineDelta(metrics);

            totalReward = (baselineDelta * 2) + currentDelta;

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

            result = (metrics.BaselineLatency - metrics.LatencyAfterAction) / metrics.BaselineLatency;

            return result;
        }

        float GetCurrentDelta(PerformanceMetrics metrics)
        {
            float result = 0;

            result = (metrics.LatencyBeforeAction - metrics.LatencyAfterAction) / metrics.LatencyBeforeAction;

            return result;
        }

    }
}