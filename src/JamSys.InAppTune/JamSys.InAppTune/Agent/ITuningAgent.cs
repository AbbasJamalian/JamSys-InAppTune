using JamSys.InAppTune.DQN;
using JamSys.InAppTune.Knobs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamSys.InAppTune.Agent
{
    public interface ITuningAgent
    {
        IKnobProvider KnobProvider { get; }

        public ExperienceReplay ExperienceReplay { get; }

        void Configure(Action<AgentConfiguration> configAction);

        void ConfigureKnobs(string databaseProvider, List<Knob> knobs);

        public void ConfigureDatabase(Action<DbContextOptionsBuilder> builderAction);

        void ActivateAutomaticTuningMode();

        void StartWorkload();

        void EndWorkload(ulong duration, float throughput);

        float Train();

        void ResetNetwork();

        void ManualTune();

        void Monitor();

        void Save(string filename);

        Task Load(string filename);

        AgentStatus GetStatus();
    }
}
