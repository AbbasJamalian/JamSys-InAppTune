using System;
using System.Collections.Generic;

namespace JamSys.InAppTune.Host.Data
{
    public class Order
    {
        public DateTime OEntryD { get; set; }
        public int OId { get; set; }
        public int OWId { get; set; }
        public int OCId { get; set; }
        public short ODId { get; set; }
        public short? OCarrierId { get; set; }
        public short OOlCnt { get; set; }
        public short OAllLocal { get; set; }
    }
}
