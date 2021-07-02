using Chep.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Chep.DTO
{
    public class WorkOrderMasterModelDTO
    {
        public VwWsWorkOrderMasterDTO VwWsWorkOrderMaster { get; set; }
        public List<VwWsWorkOrderInvoiceDTO> VwWsWorkOrderInvoices { get; set; }
        public List<VwWsWorkOrderInvoiceDetailsDTO> VwWsWorkOrderInvoiceDetails { get; set; }
    }
}
