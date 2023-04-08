using JamSys.InAppTune.Agent;

namespace JamSys.InAppTune.Rewards
{
    public interface IRewardFunction
    {
        float CalculateReward(PerformanceMetrics metrics);
    }
}
