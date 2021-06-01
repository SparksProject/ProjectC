using System;
using System.Collections.Generic;

namespace Chep.DTO
{
    public class ChepStokCikisDTO
    {
        public int StokCikisId { get; set; }
        public int ReferansNo { get; set; }
        public DateTime? IslemTarihi { get; set; }
        public DateTime? BeyannameTarihi { get; set; }
        public DateTime? TpsTarih { get; set; }
        public string BeyannameNo { get; set; }
        public Guid? IhracatciFirma { get; set; }
        public string TpsNo { get; set; }
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

        public string IhracatciFirmaName { get; set; }
        public string IslemTarihiDisplay => $"{IslemTarihi:dd.MM.yyyy}";
        public string TpsTarihDisplay => $"{TpsTarih:dd.MM.yyyy}";
        public string BeyannameTarihiDisplay => $"{BeyannameTarihi:dd.MM.yyyy}";

        public List<ChepStokCikisDetayDTO> ChepStokCikisDetayList { get; set; }
        public List<int> DeletedChepStokCikisDetayIdList { get; set; }
    }
}
