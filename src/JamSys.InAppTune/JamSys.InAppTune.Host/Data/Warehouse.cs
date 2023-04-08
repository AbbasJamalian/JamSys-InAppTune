using System;
using System.Collections.Generic;

namespace JamSys.InAppTune.Host.Data
{
    public class Warehouse
    {
        public int WId { get; set; }
        public string WName { get; set; }
        public string WStreet1 { get; set; }
        public string WStreet2 { get; set; }
        public string WCity { get; set; }
        public string WState { get; set; }
        public string WZip { get; set; }
        public decimal WTax { get; set; }
        public decimal WYtd { get; set; }
    }
}
