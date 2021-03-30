using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjectC.Data.Models
{
    public class ChepStokGiris
    {
        public ChepStokGiris()
        {
            ChepStokGirisDetayList = new HashSet<ChepStokGirisDetay>();
        }


        [Key]
        public int StokGirisId { get; set; }
        public string ReferansNo { get; set; }
        public string TPSNo { get; set; }
        public string TPSDurum { get; set; }
        public DateTime BasvuruTarihi { get; set; }
        public DateTime SureSonuTarihi { get; set; }
        public string GumrukKod { get; set; }
        public string BeyannameNo { get; set; }
        public DateTime BeyannameTarihi { get; set; }
        public string BelgeAd { get; set; }
        public string BelgeSart { get; set; }
        public string TPSAciklama { get; set; }
        public int IthalatciFirma { get; set; }
        public int IhracatciFirma { get; set; }
        public int KapAdet { get; set; }


        public virtual ICollection<ChepStokGirisDetay> ChepStokGirisDetayList { get; set; }
    }
}