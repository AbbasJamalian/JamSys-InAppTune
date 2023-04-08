using JamSys.InAppTune.Agent;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Text.RegularExpressions;

namespace JamSys.InAppTune
{
    public class TunerDbContext : DbContext
    {
        public int CommulativeLatency { get; set; }

        public ITuningAgent Agent { get; private set; }

        public TunerDbContext()
        {

        }

        public TunerDbContext(DbContextOptions options) : base(options)
        {
            Agent = TuningAgent.Instance;
        }

        public override void Dispose()
        {
            base.Dispose();
        }

        public string RawSQL(string statement)
        {
            string result;
            using (var command = this.Database.GetDbConnection().CreateCommand())
            { 
                command.CommandText = statement;
                using (var reader = command.ExecuteReader())
                {
                    result = reader.ToString();
                }
            }
            return result;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.EnableSensitiveDataLogging();
            optionsBuilder.LogTo(s => CollectData(s), LogLevel.Information);
            base.OnConfiguring(optionsBuilder);
        }

        protected void CollectData(string statements)
        {
            try
            {
                if (IsContextInitialization(statements))
                {
                    CommulativeLatency = 0;
                }
                else
                {
                    int duration = GetExecutionDuration(statements);
                    if (duration > 0)
                    {
                        CommulativeLatency += duration;
                    }
                }
                Console.WriteLine($"Current Latency: {CommulativeLatency}");
            }
            catch(Exception ex)
            {
                Console.WriteLine($"ERROR IN COLLECT DATA: {ex.Message}");
            }
        }

        private int GetExecutionDuration(string trace)
        {
            int result = -1;

            //Regex rx = new Regex(@"[\a - z\r\n\()][ Executed DbCommand \(]\d*ms\)", RegexOptions.Compiled);
            Regex rx = new Regex(@"[\a - z\r\n\()][ Executed DbCommand \(][0-9,]*ms\)", RegexOptions.Compiled);

            var matches = rx.Matches(trace);

            if (matches.Count > 0)
            {
                string duration = matches[0].Value;
                result = ParseElapsedTime(duration);
            }

            return result;
        }

        private bool IsContextInitialization(string trace)
        {
            Regex rx = new Regex(@"[\a - z\r\n\()]CoreEventId.ContextInitialized", RegexOptions.Compiled);

            var matches = rx.Matches(trace);
            return matches.Count > 0;
        }

        int ParseElapsedTime(string input)
        {
            int result = 0;

            Regex rx = new Regex(@"\d+\,?\d+");

            var matches = rx.Matches(input);

            if (matches.Count > 0)
            {
                string elapsedTime = matches[0].Value;
                if (elapsedTime.Contains(','))
                    elapsedTime = elapsedTime.Remove(elapsedTime.IndexOf(','), 1);
                result = int.Parse(elapsedTime);
            }
            /*
            string duration = input;
            duration = duration.Substring(2, duration.IndexOf('m') - 2);
            duration = duration.Remove(duration.IndexOf(','), 1);
            result = int.Parse(duration);
            */

            return result;
        }
    }

}