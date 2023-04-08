namespace JamSys.InAppTune.Agent
{
    public class PerformanceMetrics
    {
        public float BaselineLatency { get; set; }

        public float MinLatency { get; set; }

        public float LatencyBeforeAction { get; set; }

        public float LatencyAfterAction { get; set; }

        public float BaselineThroughput { get; set; }

        public float MaxThroughput { get; set; }

        public float ThroughputBeforeAction { get; set; }

        public float ThroughputAfterAction { get; set; }

        public List<float> KnobsBeforeAction { get; set; } = new();

        public List<float> KnobsAfterAction { get; set; } = new();
    }
}
