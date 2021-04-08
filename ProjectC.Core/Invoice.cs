using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ProjectC.Core
{
    public partial class Invoice
    {
        public Invoice()
        {
            InvoiceDetail = new HashSet<InvoiceDetail>();
        }

        public Guid InvoiceId { get; set; }
        public Guid WorkOrderMasterId { get; set; }
        public string SenderNo { get; set; }
        public string SenderName { get; set; }
        public string SenderAddress { get; set; }
        public string SenderCity { get; set; }
        public string SenderCountry { get; set; }
        public string ConsgnNo { get; set; }
        public string ConsgnName { get; set; }
        public string ConsgnAddress { get; set; }
        public string ConsgnCity { get; set; }
        public string ConsgnCountry { get; set; }
        public string TransptrName { get; set; }
        public string VesselName { get; set; }
        public string AgentName { get; set; }
        public string PlateNo { get; set; }
        public string AwbNo { get; set; }
        public string Blno { get; set; }
        public string Incoterms { get; set; }
        public string DeliveryLocation { get; set; }
        public decimal InvoiceAmount { get; set; }
        public string InvoiceCurrency { get; set; }
        public decimal? FreightAmount { get; set; }
        public string FreightCurrency { get; set; }
        public decimal? InsuranceAmount { get; set; }
        public string InsuranceCurrency { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ContainerNo { get; set; }
        public string GtbReferenceNo { get; set; }
        public string PaymentMethod { get; set; }
        public string EntryExitCustoms { get; set; }
        public string Customs { get; set; }

        public virtual WorkOrderMaster WorkOrderMaster { get; set; }
        public virtual ICollection<InvoiceDetail> InvoiceDetail { get; set; }
    }
}
