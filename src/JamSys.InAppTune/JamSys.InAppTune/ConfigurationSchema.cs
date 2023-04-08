using JamSys.InAppTune.Agent;
using JamSys.InAppTune.DQN;

namespace JamSys.InAppTune
{
    public class ConfigurationSchema
    {
        public AgentConfiguration AgentConfiguration { get; set; }

        public Dictionary<string, ulong> Knobs { get; set; }

        public List<ExperienceReplayEntry> ExperienceReplayEntries { get; set; }
    }
}