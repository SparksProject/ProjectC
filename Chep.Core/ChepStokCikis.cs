using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
{
    public partial class ChepStokCikis
    {
        public ChepStokCikis()
        {
            ChepStokCikisDetay = new HashSet<ChepStokCikisDetay>();
        }

        public int StokCikisId { get; set; }
        public int ReferansNo { get; set; }
        public DateTime? IslemTarihi { get; set; }
        public string BeyannameNo { get; set; }
        public DateTime? BeyannameTarihi { get; set; }
        public Guid? IhracatciFirma { get; set; }
        public string TpsNo { get; set; }
        public DateTime? TpsTarih { get; set; }
        public Guid? WorkOrderMasterId { get; set; }
        public Guid? InvoiceId { get; set; }
        public string InvoiceNo { get; set; }
        public DateTime? InvoiceDate { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public string InvoiceCurrency { get; set; }
        public string GtbReferenceNo { get; set; }
        public Guid? AliciFirma { get; set; }
        public string TeslimSekli { get; set; }
        public string CikisGumruk { get; set; }
        public string OdemeSekli { get; set; }
        public Guid? NakliyeciFirma { get; set; }
        public string CikisAracKimligi { get; set; }

        public virtual Customer IhracatciFirmaNavigation { get; set; }
        public virtual ICollection<ChepStokCikisDetay> ChepStokCikisDetay { get; set; }
    }
}
