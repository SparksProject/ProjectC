using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
{
    public partial class ChepStokGiris
    {
        public ChepStokGiris()
        {
            ChepStokGirisDetay = new HashSet<ChepStokGirisDetay>();
        }

        public int StokGirisId { get; set; }
        public string ReferansNo { get; set; }
        public string TpsNo { get; set; }
        public string TpsDurum { get; set; }
        public DateTime? BasvuruTarihi { get; set; }
        public DateTime? SureSonuTarihi { get; set; }
        public string GumrukKod { get; set; }
        public string BeyannameNo { get; set; }
        public DateTime? BeyannameTarihi { get; set; }
        public string BelgeAd { get; set; }
        public string BelgeSart { get; set; }
        public string TpsAciklama { get; set; }
        public Guid? IthalatciFirma { get; set; }
        public Guid? IhracatciFirma { get; set; }
        public int? KapAdet { get; set; }

        public virtual Customer IhracatciFirmaNavigation { get; set; }
        public virtual Customer IthalatciFirmaNavigation { get; set; }
        public virtual ICollection<ChepStokGirisDetay> ChepStokGirisDetay { get; set; }
    }
}
