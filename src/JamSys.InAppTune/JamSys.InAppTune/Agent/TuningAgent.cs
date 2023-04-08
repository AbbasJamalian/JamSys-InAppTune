using JamSys.InAppTune.DQN;
using JamSys.InAppTune.Knobs;
using JamSys.InAppTune.Rewards;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static TorchSharp.torch;

namespace JamSys.InAppTune.Agent
{
    public class TuningAgent : ITuningAgent
    {
        public enum StateEnum
        {
            Monitoring,
            TuningInit,
            MeasureBaseline, //Measure Baseline performance
            PerformAction, //Perform random actions and collect enought data for training
            MeasureReward, //Measures the reward based on the latency after action is performed
            Finish,
        }
        #region Singleton
        private static TuningAgent _instance;
        public static TuningAgent Instance
        {
            get
            {
                if (_instance == null)
                    _instance = new TuningAgent();
                return _instance;
            }
        }
        #endregion

        public Subject<(string knobName, ulong value)> KnobChanged { get; private set; }
        public Subject<(float loss, float accuracy, int epoche)> TrainingStatusChanged { get; private set; }
        public Subject<AgentStatus> StatusChanged

        { get; private set; }

        public IKnobProvider KnobProvider { get; private set; }

        public ExperienceReplay ExperienceReplay { get { return _experienceReplay; } }

        private DbContextOptionsBuilder<TunerDbContext> _options;

        public AgentConfiguration Config { get; set; } = new();

        private ExperienceReplay _experienceReplay = new();

        private IDeepQNet _qNet;

        //Configuration
        private int _epsilonGreedyPercentage = 100;

        private object _locker = new object();

        private StateEnum _state;

        private float _reward = 0;

        private List<ulong> _bestKnobValues = new();
        private List<ulong> _initialKnobValues = new();

        private bool _knobInRange; //indicates if knob is set to its min or max value

        private int _pendingAction;

        //Training
        private int _currentEpoche = 0;

        //Episodes
        private int _currentEpisode = 1;

        private IRewardFunction _rewardFunction;
        private AgentStatus _currentStatus;

        private ulong _previousLatency;
        private float _previousThroughput;

        private float _resourcesBaseline;
        private float _currentResources;

        public PerformanceMetrics Metrics { get; set; } = new();

        private TuningAgent()
        {
            KnobChanged = new();
            TrainingStatusChanged = new();
            StatusChanged = new();
            _state = StateEnum.Monitoring;
        }

        public void Configure(Action<AgentConfiguration> configAction)
        {
            Config = new AgentConfiguration();
            configAction.Invoke(Config);
        }

        public void ConfigureKnobs(string databaseProvider, List<Knob> knobs)
        {
            //ScaleKnobs(knobs, 500000, 0.5);
            
            _qNet?.Dispose();

            if (databaseProvider.Equals("MySQL"))
                KnobProvider = new MySqlKnobProvider();
            if (databaseProvider.Equals("Postgres"))
                KnobProvider = new PostgresKnobProvider();

            KnobProvider.ConfigureKnobs(knobs);
            _qNet = new DeepQNetTorch(KnobProvider.Knobs.Count, Config.NetworkTopology);

            using (var context = new TunerDbContext(_options.Options))
            {
                KnobProvider.FetchAll(context);
            }
        }

        private void ScaleKnobs(List<Knob> knobs, ulong threshold, double scale)
        {
            knobs.ToList().ForEach(k => 
            {
                if(k.Maximum > threshold)
                    k.Maximum = (ulong)(k.Maximum * scale);
            });
        }

        public void ConfigureDatabase(Action<DbContextOptionsBuilder> builderAction)
        {
            _options = new DbContextOptionsBuilder<TunerDbContext>();
            builderAction.Invoke(_options);
        }

        private void SetState(StateEnum newState)
        {
            _state = newState;
        }

        public void ActivateAutomaticTuningMode()
        {
            lock (_locker)
            {
                _currentEpisode = 1;
                _epsilonGreedyPercentage = 100;
                KnobProvider.Knobs.ToList().ForEach(k => _initialKnobValues.Add(k.Value));

                _resourcesBaseline = 0;
                KnobProvider.Knobs.ToList().ForEach(k => _resourcesBaseline += k.PercentageValue);

                var factory = new RewardFunctionFactory();
                _rewardFunction = factory.CreateRewardFunction(Config.RewardFunction);

                _qNet?.Dispose();
                _qNet = new DeepQNetTorch(KnobProvider.Knobs.Count, Config.NetworkTopology);


                SetState(StateEnum.TuningInit);
            }
        }
        public void ManualTune()
        {
            _epsilonGreedyPercentage = 5;
            EpsilonGreedyAction();
        }

        public void Monitor()
        {
            lock (_locker)
            {
                SetState(StateEnum.Monitoring);
            }
        }

        public void StartWorkload()
        {
            lock (_locker)
            {
                switch (_state)
                {
                    case StateEnum.TuningInit:

                        InitializeEpisode(_currentEpisode);
                        break;
                    case StateEnum.PerformAction:
                        EpsilonGreedyAction();
                        break;
                    case StateEnum.Finish:
                        break;
                    case StateEnum.Monitoring:
                        break;
                    default:
                        break;
                }
            }
        }

        public void EndWorkload(ulong latency, float throughput)
        {
            lock (_locker)
            {
                if (latency == 0)
                {
                    // In the case error happened, we keep the last seen latency and try to move forwrd
                    latency = _previousLatency;
                    throughput = _previousThroughput;
                }
                else
                {
                    _previousLatency = latency;
                    _previousThroughput = throughput;
                }

                switch (_state)
                {
                    case StateEnum.TuningInit:
                        SetState(StateEnum.MeasureBaseline);
                    break;
                    case StateEnum.MeasureBaseline:
                        if (_currentEpisode == 1)
                        {
                            Metrics.BaselineLatency = latency;
                            Metrics.MinLatency = latency;

                            Metrics.BaselineThroughput = throughput;
                            Metrics.MaxThroughput = throughput;
                        }

                        Metrics.LatencyBeforeAction = latency;
                        Metrics.ThroughputBeforeAction = throughput;

                        UpdateStatus("", latency, throughput);
                        SetState(StateEnum.PerformAction);
                        break;
                    case StateEnum.PerformAction:
                        SetState(StateEnum.MeasureReward);
                        break;
                    case StateEnum.MeasureReward:
                        if (Metrics.MinLatency > latency)
                        {
                            _bestKnobValues.Clear();
                            KnobProvider.Knobs.ToList().ForEach(k => _bestKnobValues.Add(k.Value));
                            Metrics.MinLatency = latency;
                        }

                        if (throughput > Metrics.MaxThroughput)
                        {
                            Metrics.MaxThroughput = throughput;
                        }

                        Metrics.LatencyAfterAction = latency;
                        Metrics.ThroughputAfterAction = throughput;

                        _reward = _rewardFunction.CalculateReward(Metrics);
                        _experienceReplay.Add(Metrics.KnobsBeforeAction, _pendingAction, _reward, Metrics.KnobsAfterAction, Metrics.LatencyAfterAction);

                        if (_experienceReplay.Size % Config.ExplorationsPerEpisode == 0)
                        {
                            _currentEpisode++;

                            if (_currentEpisode <= Config.NumberOfEpisodes)
                                SetState(StateEnum.TuningInit);
                            else
                            {
                                SetState(StateEnum.Finish);
                                if (Config.SetBestValuesOnFinish)
                                    SetAllKnobs(_bestKnobValues);

                            }
                        }
                        else
                        {
                            //The call to calculate reward should have been happened before next two lines are set
                            Metrics.LatencyBeforeAction = latency;
                            Metrics.ThroughputBeforeAction = throughput;

                            SetState(StateEnum.PerformAction);
                        }
                        UpdateStatus("", latency, throughput);

                        if (_experienceReplay.Size > Config.ExperienceReplayThreshold && Config.TrainDuringDataCollection)
                        {
                            Train();
                        }
                        break;
                    case StateEnum.Finish:
                        UpdateStatus("", latency, throughput);
                        break;
                    case StateEnum.Monitoring:
                        UpdateStatus("", latency, throughput);
                        break;
                    default:
                        break;
                }
            }
        }


        private void EpsilonGreedyAction()
        {
            int action;
            Random random = new Random();

            bool performRandomAction = random.Next(1, 100) <= _epsilonGreedyPercentage;

            if (performRandomAction)
            {
                action = random.Next(0, KnobProvider.Knobs.Count * 2);
                if (action > 0 && action % 2 != 0)
                {
                    //This is a decreasing action. 
                    bool makeItIncreasing = random.Next(1, 100) <= Config.WeightIncreasingKnobs; //weight for increasing knobs
                    if (makeItIncreasing)
                        action--;
                }
                Console.WriteLine($"Epsilon Greedy Rndom Action: {action}, Epsilon: {_epsilonGreedyPercentage}");
            }
            else
            {
                List<float> currentState = new();
                KnobProvider.Knobs.ToList().ForEach(k => currentState.Add(k.PercentageValue));
                action = (int)_qNet.GetAction(currentState);
                Console.WriteLine($"Epsilon Greedy Network Action: {action}, Epsilon: {_epsilonGreedyPercentage}");
            }

            PerformAction(action);

            if (!performRandomAction && !_knobInRange)
            {
                //QNet is locked, we perform a random action
                action = random.Next(0, KnobProvider.Knobs.Count * 2);
                PerformAction(action);
                Console.WriteLine($"Epsilon Greedy Network locked, performing random Action: {action}");
            }

        }

        public float Train()
        {
            float loss = 0;

            try
            {
                int batchSize = Config.TrainingsPerIteration;
                for (int i = 0; i < batchSize; i++)
                {
                    var batch = _experienceReplay.GetRandomBatch(1);
                    var trainingSample = batch[0];
                    loss += _qNet.TrainSingle(
                        trainingSample.currentState,
                        trainingSample.action,
                        trainingSample.reward,
                        trainingSample.nextState,
                        (float)Convert.ToDouble(Config.LearningRate),
                        (float)Convert.ToDouble(Config.DiscountFactor));
                }

                loss /= batchSize;

                _currentEpoche++;

                if (_currentEpoche % Config.EpochesToCopyQNet == 0)
                {
                    //Copy QNet to TargetNet
                    _qNet.CopyQNetToTarget();
                }

                float accuracy = MeasureAccuracy();

                Task.Run(() => TrainingStatusChanged.OnNext((loss, accuracy, _currentEpoche)));

                if (_epsilonGreedyPercentage > 0 && accuracy > Config.AccuracyThresholdForEpsilonreedy)
                {
                    _epsilonGreedyPercentage--;
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error Training Network: {ex.Message}");
            }


            return loss;
        }

        private void UpdateStatus(string log, float latency, float throughput, float accuracy = 0)
        {
            var currentLatency = latency;

            int progress = (_experienceReplay.Size * 100) / (Config.NumberOfEpisodes * Config.ExplorationsPerEpisode);

            float currentResources = 0;
            KnobProvider.Knobs.ToList().ForEach(k => currentResources += k.PercentageValue);


            _currentStatus = new AgentStatus()
            {
                ExperienceReplaySize = _experienceReplay.Size,
                AgentState = _state.ToString(),
                BaselineLatency = Metrics.BaselineLatency,
                CurrentLatency = currentLatency,
                Reward = _reward,
                LatestAction = _pendingAction,
                Message = log,
                Accuracy = accuracy,
                CurrentEpisode = _currentEpisode,
                DataCollectionProgress = progress,
                Throughput = throughput,
                EpsilonGreedyRate = _epsilonGreedyPercentage,
                ResourceUtilization = (currentResources - _resourcesBaseline) / _resourcesBaseline
            };

            Task.Run(() => StatusChanged.OnNext(_currentStatus));
        }

        public AgentStatus GetStatus()
        {
            return _currentStatus;
        }


        /// <summary>
        /// Action / 2 identifies the Knob Index, Even Actions will increase the knob and odd actions decrease the knob
        /// </summary>
        /// <param name="actionIdentifier"></param>
        private void PerformAction(int actionIdentifier)
        {
            try
            {
                int index = actionIdentifier / 2;
                bool increase = actionIdentifier % 2 == 0;

                if (index <= KnobProvider.Knobs.Count)
                {
                    //_currentEnvState.Clear();
                    //KnobProvider.Knobs.ToList().ForEach(k => _currentEnvState.Add(k.PercentageValue));
                    Metrics.KnobsBeforeAction.Clear();
                    KnobProvider.Knobs.ToList().ForEach(k => Metrics.KnobsBeforeAction.Add(k.PercentageValue));

                    ulong currentValue = KnobProvider.Knobs[index].Value;

                    _knobInRange = true;

                    if (increase)
                    {
                        //If we are already at maximum
                        if (currentValue >= KnobProvider.Knobs[index].Maximum)
                            _knobInRange = false;

                        currentValue += KnobProvider.Knobs[index].Step;
                        if (currentValue > KnobProvider.Knobs[index].Maximum)
                        {
                            currentValue = KnobProvider.Knobs[index].Maximum;
                        }
                    }
                    else
                    {
                        if (currentValue <= KnobProvider.Knobs[index].Minimum)
                            _knobInRange = false;

                        if (KnobProvider.Knobs[index].Step >= currentValue) //Because it is unsigned operation
                        {
                            currentValue = KnobProvider.Knobs[index].Minimum;
                        }
                        else
                            currentValue -= KnobProvider.Knobs[index].Step;
                    }

                    using (var context = new TunerDbContext(_options.Options))
                    {
                        KnobProvider.SetKnobValue(context, KnobProvider.Knobs[index].Name, currentValue);
                    }

                    Thread.Sleep((int)KnobProvider.Knobs[index].SafetyDelay); //Safety time after setting a knob

                    ulong valueAfterSet;
                    using (var context = new TunerDbContext(_options.Options))
                    {
                        valueAfterSet = KnobProvider.FetchKnobValue(context, KnobProvider.Knobs[index].Name);
                    }

                    if (valueAfterSet != currentValue)
                    {
                        Console.WriteLine("Knob Value has not been accepted");
                        //KnobProvider.Knobs[index].Value = valueAfterSet;
                    }

                    //_nextEnvState.Clear();
                    //KnobProvider.Knobs.ToList().ForEach(k => _nextEnvState.Add(k.PercentageValue));
                    Metrics.KnobsAfterAction.Clear();
                    KnobProvider.Knobs.ToList().ForEach(k => Metrics.KnobsAfterAction.Add(k.PercentageValue));

                    _pendingAction = actionIdentifier;

                    Task.Run(() => KnobChanged.OnNext((KnobProvider.Knobs[index].Name, valueAfterSet)));
                }
                else
                    Console.WriteLine($"Invalid Action requested: {actionIdentifier}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR in TuningAgent.PerformAction: {ex.Message}");
            }
        }

        private void InitializeEpisode(int episode)
        {
            bool doRandom = Config.InitializeEpisodeRandom;
            int knobIndex = 0;
            Random random = new Random();
            foreach (var knob in KnobProvider.Knobs)
            {
                using (var context = new TunerDbContext(_options.Options))
                {
                    ulong value;
                    //ulong value = episode == 1 ? knob.DefaultValue : knob.Step * (ulong)episode * 5;
                    //ulong value = knob.DefaultValue;
                    //ulong value = episode < 3 ? knob.DefaultValue : (ulong)random.NextInt64((long)knob.Minimum, (long)knob.Maximum);
                    if (doRandom)
                    {
                        value = (ulong)random.NextInt64((long)knob.Minimum, (long)knob.Maximum);
                    }
                    else
                    {
                        //value = episode == 1 && level <= 2 ? knob.DefaultValue : ((ulong)level - 1) * (knob.Maximum - knob.Minimum) / (ulong)Config.LevelsPerEpisode;
                        value = _initialKnobValues[knobIndex++];
                    }

                    KnobProvider.SetKnobValue(context, knob.Name, value);
                    Task.Run(() => KnobChanged.OnNext((knob.Name, value)));
                }
                Thread.Sleep(500);
            }
        }

        private void SetAllKnobs(List<ulong> knobs)
        {
            int knobIndex = 0;
            foreach (var knob in KnobProvider.Knobs)
            {
                using (var context = new TunerDbContext(_options.Options))
                {
                    ulong value;
                    if (knobIndex < knobs.Count)
                    {
                        value = knobs[knobIndex++];
                        KnobProvider.SetKnobValue(context, knob.Name, value);
                        Task.Run(() => KnobChanged.OnNext((knob.Name, value)));
                    }
                }
                Thread.Sleep(500);
            }
        }


        private float MeasureAccuracy()
        {
            int totalPositiveCorrect = 0;
            int totalNegativeCorrect = 0;
            int totalPositive = 0;
            int totalNegative = 0;
            for (int i = 0; i < _experienceReplay.Size; i++)
            {
                var entry = _experienceReplay.GetItem(i);
                long action = _qNet.GetAction(entry.CurrentState);

                if (entry.Reward >= 0)
                {
                    totalPositive++;
                    totalPositiveCorrect += action == (long)entry.Action ? 1 : 0;
                }
                else if (entry.Reward < 0)
                {
                    totalNegative++;
                    totalNegativeCorrect += action != (long)entry.Action ? 1 : 0;
                }

            }

            //float accuracy = (totalPositiveCorrect + totalNegativeCorrect) * 100 / (totalPositive + totalNegative);
            float accuracy = (totalPositiveCorrect) * 100 / (totalPositive);
            return accuracy;
        }

        public void DumpNetwork()
        {
            int totalCorrectActions = 0;
            List<long> actions = new();
            for (int i = 0; i < _experienceReplay.Size; i++)
            {
                var entry = _experienceReplay.GetItem(i);
                long action = _qNet.GetAction(entry.CurrentState);
                actions.Add(action);

                if (entry.Reward > 0)
                {
                    totalCorrectActions += action == (long)entry.Action ? 1 : 0;
                }
                else if (entry.Reward < 0)
                {
                    totalCorrectActions += action != (long)entry.Action ? 1 : 0;
                }
                else
                    totalCorrectActions++;

            }
            string output = "ACTIONS: ";
            actions.ForEach(a => output += a.ToString() + ",");
            Console.WriteLine(output);

            var accuracy = (totalCorrectActions * 100 / _experienceReplay.Size);
        }

        public void Save(string filename)
        {
            ConfigurationSchema config = new();
            config.AgentConfiguration = this.Config;
            config.Knobs = new();
            KnobProvider.Knobs.ForEach(k => config.Knobs.Add(k.Name, k.Value));

            config.ExperienceReplayEntries = new();
            config.ExperienceReplayEntries.AddRange(_experienceReplay.GetEntries());

            string json = JsonSerializer.Serialize<ConfigurationSchema>(config, new JsonSerializerOptions() { WriteIndented = true }); //(schema, );

            File.WriteAllText(filename, json);

        }

        public async Task Load(string filename)
        {
            await Task.Run(() => {
                if (File.Exists(filename))
                {
                    string json = File.ReadAllText(filename);

                    var config = JsonSerializer.Deserialize<ConfigurationSchema>(json);

                    this.Config = config.AgentConfiguration;
                    _experienceReplay.LoadEntries(config.ExperienceReplayEntries);

                    foreach (var knob in config.Knobs)
                    {
                        using (var context = new TunerDbContext(_options.Options))
                        {
                            ulong value;
                            value = knob.Value;
                            KnobProvider.SetKnobValue(context, knob.Key, value);
                            Task.Run(() => KnobChanged.OnNext((knob.Key, value)));
                        }
                        Thread.Sleep(500);
                    }
                    ResetNetwork();
                }
            });
        }

        public void ResetNetwork()
        {
            _currentEpoche = 0;
            //_qNet.Reset();

            _qNet?.Dispose();
            _qNet = new DeepQNetTorch(KnobProvider.Knobs.Count, Config.NetworkTopology);
        }
    }
}
