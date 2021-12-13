using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
{
    public partial class VwWsWorkOrderInvoiceDetailsTcgb
    {
        public int StokCikisId { get; set; }
        public Guid? InvoiceDetailsTcgbId { get; set; }
        public Guid? InvoiceDetailId { get; set; }
        public string DeclarationNo { get; set; }
        public int? ItemNo { get; set; }
        public int? Quantity { get; set; }
        public string Description { get; set; }
        public DateTime? DeclarationDate { get; set; }
    }
}
