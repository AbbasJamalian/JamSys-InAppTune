using System;
using System.Collections.Generic;

namespace JamSys.InAppTune.Host.Data
{
    public class OrderLine
    {
        public DateTime? OlDeliveryD { get; set; }
        public int OlOId { get; set; }
        public int OlWId { get; set; }
        public int OlIId { get; set; }
        public int OlSupplyWId { get; set; }
        public short OlDId { get; set; }
        public short OlNumber { get; set; }
        public short OlQuantity { get; set; }
        public decimal? OlAmount { get; set; }
        public string OlDistInfo { get; set; }
    }
}
