using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ProjectC.Core
{
    public partial class WorkOrderSend
    {
        public int WorkOrderSendId { get; set; }
        public DateTime Insdate { get; set; }
        public string Xmltext { get; set; }
        public string Expno { get; set; }
        public string Statu { get; set; }
        public string Dosyano { get; set; }
        public byte Processstatus { get; set; }
        public DateTime? Upddate { get; set; }
        public string WorkOrderNo { get; set; }
        public Guid? WorkOrderMasterId { get; set; }
    }
}
