using System;
using System.Collections.Generic;

namespace JamSys.InAppTune.Host.Data
{
    public class Customer
    {
        public DateTime CSince { get; set; }
        public int CId { get; set; }
        public int CWId { get; set; }
        public short CDId { get; set; }
        public short CPaymentCnt { get; set; }
        public short CDeliveryCnt { get; set; }
        public string CFirst { get; set; }
        public string CMiddle { get; set; }
        public string CLast { get; set; }
        public string CStreet1 { get; set; }
        public string CStreet2 { get; set; }
        public string CCity { get; set; }
        public string CState { get; set; }
        public string CZip { get; set; }
        public string CPhone { get; set; }
        public string CCredit { get; set; }
        public decimal CCreditLim { get; set; }
        public decimal CDiscount { get; set; }
        public decimal CBalance { get; set; }
        public decimal CYtdPayment { get; set; }
        public string CData { get; set; }
    }
}
