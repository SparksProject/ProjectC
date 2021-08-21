using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
{
    public partial class VwWsWorkOrderInvoiceDetails
    {
        public int StokCikisId { get; set; }
        public Guid? InvoiceId { get; set; }
        public Guid? WorkOrderMasterId { get; set; }
        public int WorkOrderNo { get; set; }
        public Guid? InvoiceDetailId { get; set; }
        public string HsCode { get; set; }
        public string DescGoods { get; set; }
        public string ProductNo { get; set; }
        public string CountryOfOrigin { get; set; }
        public string Uom { get; set; }
        public int? ActualQuantity { get; set; }
        public int? InvoiceQuantity { get; set; }
        public decimal GrossWeight { get; set; }
        public decimal NetWeight { get; set; }
        public string IntrnlAgmt { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string PkgType { get; set; }
        public string CommclDesc { get; set; }
        public int NumberOfPackages { get; set; }
        public long? ItemNumber { get; set; }
        public string ProducerCompanyNo { get; set; }
        public string ProducerCompany { get; set; }
        public string IncentiveLineNo { get; set; }
    }
}
