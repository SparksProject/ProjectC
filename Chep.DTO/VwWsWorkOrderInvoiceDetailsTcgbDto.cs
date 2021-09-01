using System;
using System.Collections.Generic;
using System.Text;

namespace Chep.DTO
{
    public class VwWsWorkOrderInvoiceDetailsTcgbDto
    {
        public int StokCikisId { get; set; }
        public Guid InvoiceDetailsTcgbId { get; set; }
        public Guid InvoiceDetailId { get; set; }
        public string DeclarationNo { get; set; }
        public int ItemNo { get; set; }
        public decimal Quantity { get; set; }
        public string Description { get; set; }
        public DateTime? DeclarationDate { get; set; }
    }
}
