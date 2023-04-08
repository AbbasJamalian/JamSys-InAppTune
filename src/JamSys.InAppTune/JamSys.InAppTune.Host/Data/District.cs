using System;
using System.Collections.Generic;

namespace JamSys.InAppTune.Host.Data
{
    public class District
    {
        public int DWId { get; set; }
        public int DNextOId { get; set; }
        public short DId { get; set; }
        public decimal DYtd { get; set; }
        public decimal DTax { get; set; }
        public string DName { get; set; }
        public string DStreet1 { get; set; }
        public string DStreet2 { get; set; }
        public string DCity { get; set; }
        public string DState { get; set; }
        public string DZip { get; set; }
    }
}
