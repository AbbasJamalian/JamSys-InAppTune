using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamSys.InAppTune.Knobs
{
    public class Knob
    {
        public string Name { get; set; }

        public ulong Value { get; set; }

        public ulong DefaultValue { get; set; }

        /// <summary>
        /// Minimum Value of the Knob
        /// </summary>
        public ulong Minimum { get; set; }

        /// <summary>
        /// The Maximum value which is used by trainer
        /// </summary>
        public ulong Maximum { get; set; }

        public bool PostAction { get; set; }

        public string Scope { get; set; }

        private ulong _step;
        /// <summary>
        /// The Step to increase or decrease the knob value
        /// </summary>
        public ulong Step
        { 
            get 
            {
                if(_step == 0)
                {
                    _step = (Maximum - Minimum) / 100;
                    if(_step == 0)
                        _step = 1; 
                }
                return _step;
            }
            set
            {
                _step = value;
            } 
        }

        /// <summary>
        /// Safety delay can be set depending to the knob for the cases DBMS needs a time to apply the knob. Default value is zero and the unit is milliseconds
        /// </summary>
        public ulong SafetyDelay { get; set; }

        public float PercentageValue 
        { 
            get 
            {
                float norm = GetPercentage();
                return norm; 
            } 
        }

        public Knob()
        {
            
        }
        public Knob(string name, ulong defaultValue, ulong min, ulong max, ulong step = 0, ulong safetyDelay = 0, bool postAction = false)
        {
            Name = name;
            Minimum = min;
            Maximum = max;
            DefaultValue = defaultValue;
            Step = step == 0 ? (max - min) / 100 : step;
            SafetyDelay = safetyDelay;
            PostAction = postAction;
        }

        private float GetPercentage()
        {
            float result = ((float)(Value - Minimum) * 100) / (float)(Maximum - Minimum);
            return result;
        }

        public float ValueFromPercentage(float percentage)
        {
            return ((percentage * (float)(Maximum - Minimum)) / 100) - Minimum;
        }

    }
}
