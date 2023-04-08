using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace JamSys.InAppTune.DQN
{
    public class ExperienceReplay
    {
        private int _maxSize = 50000;
        private List<ExperienceReplayEntry> _experienceReplay = new();
        public int Size => _experienceReplay.Count;

        public bool OnlyPositiveReward { get; set; } = false;

        public bool NoDuplicate { get; set; } = false;
        public void Add(List<float> currentState, int action, float reward, List<float> nextState, float latency)
        {
            if (OnlyPositiveReward && reward < 0)
                return;

            if (NoDuplicate)
            {
                var entry = _experienceReplay.FirstOrDefault(e => ListEquals(e.CurrentState, currentState) && e.Action == action);
                if (entry != null)
                {
                    entry.Reward = (entry.Reward + reward) / 2;
                    return;
                }
            }

            if(_experienceReplay.Count > _maxSize)
                _experienceReplay.RemoveAt(0);

            _experienceReplay.Add(new ExperienceReplayEntry(currentState, action, reward, nextState, latency));
        }

        public List<(List<float> currentState, int action, float reward, List<float> nextState)> GetRandomBatch(int batchSize)
        {
            List<(List<float> currentState, int action, float reward, List<float> nextState)> result = new();

            Random random = new Random();
            for (int i = 0; i < batchSize; i++)
            {
                int index = random.Next(0, _experienceReplay.Count - 1);
                result.Add(_experienceReplay[index].AsTuple());
            }

            return result;
        }

        public ExperienceReplayEntry GetItem(int index)
        { 
            return _experienceReplay[index];
        }

        public List<ExperienceReplayEntry> GetEntries()
        {
            return _experienceReplay;
        }

        public void LoadEntries(List<ExperienceReplayEntry> entries)
        {
            _experienceReplay = new();
            _experienceReplay.AddRange(entries);
        }

        private bool ListEquals(List<float> first, List<float> second)
        {
            if (first.Count != second.Count)
                return false;

            bool result = true;
            for (int i = 0; i < first.Count; i++)
            {
                if (first[i] != second[i])
                {
                    result = false;
                    break;
                }
            }
            return result;
        }

    }

    public class ExperienceReplayEntry
    {
        public ExperienceReplayEntry()
        {

        }

        public ExperienceReplayEntry(List<float> currentState, int action, float reward, List<float> nextState, float latency)
        {
            Action = action;
            Reward = reward;
            Latency = latency;
            CurrentState = new List<float>(currentState);
            NextState = new List<float>(nextState);
        }
        public List<float> CurrentState { get; set; }
        public int Action { get; set; }
        public List<float> NextState { get; set; }
        public float Reward { get; set; }
        public float Latency { get; set; }

        public (List<float> currentState, int action, float reward, List<float> nextState) AsTuple()
        {
            return (CurrentState, Action, Reward, NextState);
        }
    }
}
