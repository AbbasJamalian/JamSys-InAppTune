using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JamSys.InAppTune.Knobs
{
    public interface IKnobProvider
    {
        List<Knob> Knobs { get; }

        void ConfigureKnobs(List<Knob> knobs);

        void SetKnobValue(DbContext context, string knobName, ulong value);

        ulong FetchKnobValue(DbContext context, string knobName);

        void FetchAll(DbContext context);
    }
}
