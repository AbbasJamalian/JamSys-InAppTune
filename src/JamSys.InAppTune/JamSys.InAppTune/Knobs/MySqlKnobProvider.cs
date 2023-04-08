using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamSys.InAppTune.Knobs
{
    internal class MySqlKnobProvider : IKnobProvider
    {
        public List<Knob> Knobs { get; private set; }

        public MySqlKnobProvider()
        {
            List<Knob> knobs = new List<Knob>();

            //Manual configuration / preparation
            //Set innodb_buffer_pool_instances = 1
            //innodb_buffer_pool_chunk_size = 1048576

            //Knobs which need restart
            //innodb_log_file_size
            //innodb_sync_array_size
            //innodb_sort_buffer_size

            //knobs.Add(new Knob() { Name = "innodb_adaptive_flushing_lwm",       DefaultValue = 10,          Minimum = 0,        DBMaximum = 70,                     Maximum = 70});
            //knobs.Add(new Knob() { Name = "innodb_adaptive_hash_index",         DefaultValue = 1,           Minimum = 0,        DBMaximum = 1,                      Maximum = 1 });
            //knobs.Add(new Knob() { Name = "innodb_adaptive_max_sleep_delay",    DefaultValue = 150000,      Minimum = 0,        DBMaximum = 1000000,                Maximum = 1000000 });
            //knobs.Add(new Knob() { Name = "innodb_io_capacity",                 DefaultValue = 200,         Minimum = 100,      DBMaximum = 18446744073709551615,   Maximum = 1000});
            //knobs.Add(new Knob() { Name = "innodb_max_dirty_pages_pct",         DefaultValue = 75,          Minimum = 0,        DBMaximum = 99,                     Maximum = 99});
            //knobs.Add(new Knob() { Name = "innodb_max_dirty_pages_pct_lwm",     DefaultValue = 0,           Minimum = 0,        DBMaximum = 99,                     Maximum = 99}); //should be always less than innodb_max_dirty_pages_pct
            //knobs.Add(new Knob() { Name = "innodb_thread_concurrency",          DefaultValue = 0,           Minimum = 0,        DBMaximum = 1000,                   Maximum = 1000}); //0 mean infinite
            //knobs.Add(new Knob() { Name = "thread_cache_size",                  DefaultValue = 16384,       Minimum = 0,        DBMaximum = 16384,                  Maximum = 16384 }); //Default value is actually -1, but documentation states not setting -1 to the parameter


            //knobs.Add(new Knob() { Name = "innodb_buffer_pool_size",            DefaultValue = 134217728,   Minimum = 5242880,  DBMaximum = 18446744073709551615,   Maximum = 4294967295, Step = 134217728, SafetyDelay = 1000 }); //8GB Max, Step is equal to innodb_buffer_pool_chunk_size (134217728)
            //knobs.Add(new Knob() { Name = "max_heap_table_size",                DefaultValue = 16777216,    Minimum = 16384,    DBMaximum = 18446744073709550592,   Maximum = 4294967295, Step = 42900000 }); //Step: 16777216
            //knobs.Add(new Knob() { Name = "tmp_table_size",                     DefaultValue = 16777216,    Minimum = 1024,     DBMaximum = 18446744073709551615,   Maximum = 4294967295, Step = 42900000 });
            //knobs.Add(new Knob() { Name = "query_cache_size",                   DefaultValue = 1048576,     Minimum = 0,        DBMaximum = 18446744073709551615,   Maximum = 4294967295, Step = 42900000 });
            //knobs.Add(new Knob() { Name = "sort_buffer_size",                   DefaultValue = 262144,      Minimum = 32768,    DBMaximum = 4294967295,             Maximum = 4294967295, Step = 42900000 }); //maximum on windows
            //knobs.Add(new Knob() { Name = "read_buffer_size",                   DefaultValue = 131072,      Minimum = 8192,     DBMaximum = 2147479552,             Maximum = 2147479552, Step = 21400000 }); 

            knobs.Add(new Knob("innodb_buffer_pool_size",   defaultValue: 134217728, min: 5242880, max: 4294967295, step: 134217728, safetyDelay: 1000)); //4GB Max, Step is equal to innodb_buffer_pool_chunk_size (134217728)
            knobs.Add(new Knob("max_heap_table_size",       defaultValue: 16777216,  min: 16384,   max: 536870912, safetyDelay: 500));
            knobs.Add(new Knob("tmp_table_size",            defaultValue: 16777216, min: 1024, max: 536870912, safetyDelay: 500)); 

            knobs.Add(new Knob("query_cache_size", defaultValue: 1048576, min: 0, max: 536870912, safetyDelay: 500));
            knobs.Add(new Knob("sort_buffer_size", defaultValue: 262144, min: 32768, max: 536870912, safetyDelay: 500));
            knobs.Add(new Knob("read_buffer_size", defaultValue: 131072, min: 8192, max: 536870912, safetyDelay: 500));

            knobs.Add(new Knob("thread_cache_size", defaultValue: 9, min: 0, max: 16384, safetyDelay: 500));
            knobs.Add(new Knob("join_buffer_size", defaultValue: 262144, min: 128, max: 536870912, safetyDelay: 500));

            Knobs = knobs;
        }

        public ulong FetchKnobValue(DbContext context, string knobName)
        {
            ulong result = 1;

            try
            {
                Knob knob = Knobs.FirstOrDefault(x => x.Name == knobName);
                using (var command = context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = "SELECT @@" + knobName;
                    context.Database.OpenConnection();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            reader.Read();
                            //result = (ulong)reader.GetInt64(0);
                            var fieldType = reader.GetFieldType(0);
                            if(fieldType.Equals(typeof(System.UInt64)))
                            {
                                result = (ulong)reader.GetInt64(0);
                            }
                            else if(fieldType.Equals(typeof(double)))
                            {
                                result = (ulong)Convert.ToInt64(reader.GetDouble(0));
                            }
                            else
                            {
                                Console.WriteLine($"ERROR: knob type is not supported {knobName}, Type: {fieldType}");   
                            }
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

                string statement = string.Format("SET {0} {1} = {2}", knob.Scope, knobName, value);
                var result = context.Database.ExecuteSqlRaw(statement);
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

        public void FetchAll(DbContext context)
        {
            Knobs.ToList().ForEach(k => FetchKnobValue(context, k.Name));
        }

        public void ConfigureKnobs(List<Knob> knobs)
        {
            Knobs.Clear();
            Knobs.AddRange(knobs);
        }

    }
}
