using System;

namespace Chep.DTO
{
    public class ChepStokCikisDetayDTO
    {
        public int StokCikisDetayId { get; set; }
        public int StokCikisId { get; set; }
        public int StokGirisDetayId { get; set; }
        public int? Miktar { get; set; }
        public decimal? Kg { get; set; }
        public Guid? InvoiceDetailId { get; set; }
        public decimal? InvoiceAmount { get; set; }
        public int? TpsCikisSiraNo { get; set; }
        public decimal? NetKg { get; set; }
        public decimal? BrutKg { get; set; }
        public decimal? BirimTutar { get; set; }
        public string UrunKod { get; set; }

    }
}
