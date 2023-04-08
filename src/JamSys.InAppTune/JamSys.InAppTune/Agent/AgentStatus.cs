using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace JamSys.InAppTune.Agent
{
    public class AgentStatus
    {
        public int ExperienceReplaySize { get; set; }
        public int CurrentEpisode { get; set; }
        public float CurrentLatency { get; set; }

        public int DataCollectionProgress { get; set; }

        public string AgentState { get; set; }
        public string Message { get; set; }
        
        public float BaselineLatency { get; set; }
        public float Reward { get; set; }
        public float Accuracy { get; set; }
        public int LatestAction { get; set; }

        public float Throughput { get; set; }

        public int EpsilonGreedyRate { get; set; }

        public float ResourceUtilization { get; set; }
    }
}
