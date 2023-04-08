using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JamSys.InAppTune.Rewards;

namespace JamSys.InAppTune.Agent
{
    public record AgentConfiguration
    {
        public List<string> AvailableRewardFunctions { get; set; } = new();
        public int NumberOfEpisodes { get; set; } = 2;
        public int ExplorationsPerEpisode { get; set; } = 100;

        public bool InitializeEpisodeRandom { get; set; } = false;

        public bool TrainDuringDataCollection { get; set; } = true;

        public int ExperienceReplayThreshold { get; set; } = 50;

        public int TrainingsPerIteration { get; set; } = 150;

        public int EpochesToCopyQNet { get; set; } = 100;

        public int AccuracyThresholdForEpsilonreedy { get; set; } = 50;

        public bool SetBestValuesOnFinish { get; set; } = true;

        public string RewardFunction { get; set; }

        public decimal? LearningRate { get; set; } = 0.000005m;

        public decimal? DiscountFactor { get; set; } = 0.9m;

        public int WeightIncreasingKnobs { get; set; } = 50;

        public string NetworkTopology { get; set; } = "Linear(250);LeakyReLU(0.2);Linear(150);LeakyReLU(0.2);Linear(100);LeakyReLU(0.2);Linear();";
        //public string NetworkTopology { get; set; }

        public AgentConfiguration()
        {
            AvailableRewardFunctions.Clear();
            var rewardFunctions = new RewardFunctionFactory().EnumerateRewardFunctions();
            if(rewardFunctions != null && rewardFunctions.Count > 0)
            {
                rewardFunctions.ForEach(t => AvailableRewardFunctions.Add(t.Name));
                RewardFunction = AvailableRewardFunctions[0];
            }
        }

    }
}
