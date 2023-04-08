using Microsoft.EntityFrameworkCore;

namespace JamSys.InAppTune.Knobs
{
    internal class PostgresKnobProvider : IKnobProvider
    {
        public List<Knob> Knobs { get; private set; }

        public PostgresKnobProvider()
        {
            Knobs = new List<Knob>();
        }

        public void ConfigureKnobs(List<Knob> knobs)
        {
            Knobs.Clear();
            Knobs.AddRange(knobs);
        }

        public void FetchAll(DbContext context)
        {
            Knobs.ToList().ForEach(k => FetchKnobValue(context, k.Name));
        }

        public ulong FetchKnobValue(DbContext context, string knobName)
        {
            ulong result = 1;

            try
            {
                Knob knob = Knobs.FirstOrDefault(x => x.Name == knobName);
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = $"select setting from pg_settings where name = '{knobName}'";
                    context.Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            var resultText = reader.GetString(0);
                            result = (ulong)long.Parse(resultText);
                            knob.Value = result;
                        }
                        else
                        {
                            Console.WriteLine($"ERROR: Unable to fetch Knob {knobName}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Unable to fetch Knob {knobName} - Message: {ex.Message}");
            }

            return result; 

        }

        public void SetKnobValue(DbContext context, string knobName, ulong value)
        {
            try
            {
                Knob knob = Knobs.FirstOrDefault(k => k.Name.Equals(knobName));
                string statement = string.Format("ALTER {0} SET {1} TO '{2}'", knob.Scope, knobName, value);

                var result = context.Database.ExecuteSqlRaw(statement);

                if(knob.PostAction)
                {
                    result = context.Database.ExecuteSqlRaw("SELECT pg_reload_conf()");
                }

                context.Database.CloseConnection();
                context.Database.OpenConnection();

                var newValue = FetchKnobValue(context, knobName);

                if (newValue != value)
                {
                    Console.WriteLine("Value is not accepted, try again");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: Unable to set Knob {knobName} - Message: {ex.Message}");
            }
        }
    }
}