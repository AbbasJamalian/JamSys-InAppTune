using System;
using System.Collections.Generic;

namespace JamSys.InAppTune.Host.Data
{
    public class History
    {
        public DateTime HDate { get; set; }
        public int? HCId { get; set; }
        public int HCWId { get; set; }
        public int HWId { get; set; }
        public short HCDId { get; set; }
        public short HDId { get; set; }
        public decimal HAmount { get; set; }
        public string HData { get; set; }
    }
}
