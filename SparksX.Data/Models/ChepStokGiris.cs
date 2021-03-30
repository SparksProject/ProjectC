using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SparksX.Data.Models
{
    public class ChepStokGiris
    {
        [Key]
        public int StokGirisId { get; set; }
        public string ReferansNo { get; set; }
        public string TPSNo { get; set; }
        public string TPSDurum { get; set; }
        public DateTime BasburuTarihi { get; set; }
        public DateTime SureSonuTarihi { get; set; }
        public string GumrukKodu { get; set; }
        public string BeyannameNo { get; set; }
        public DateTime BeyannameTarihi { get; set; }
        public string BelgeAd { get; set; }
        public string BeyannameSart { get; set; }
        public string TPSAciklama { get; set; }
        public int IthalatciFirma { get; set; }
        public int IhracatciFirma { get; set; }
        public int KapAdet { get; set; }
    }
}
