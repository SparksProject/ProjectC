using System;
using System.Collections.Generic;

namespace Chep.DTO
{
    public class ChepStokGirisDTO
    {
        public int StokGirisId { get; set; }
        public Guid? IthalatciFirma { get; set; }
        public Guid? IhracatciFirma { get; set; }
        public int? KapAdet { get; set; }
        public DateTime? BasvuruTarihi { get; set; }
        public DateTime? SureSonuTarihi { get; set; }
        public DateTime? BeyannameTarihi { get; set; }
        public string GumrukKod { get; set; }
        public string BeyannameNo { get; set; }
        public string BelgeAd { get; set; }
        public string BelgeSart { get; set; }
        public string TPSAciklama { get; set; }
        public string ReferansNo { get; set; }
        public string TPSNo { get; set; }
        public string TPSDurum { get; set; }

        public string BasvuruTarihiDisplay => $"{BasvuruTarihi:dd.MM.yyyy}";
        public string SureSonuTarihiDisplay => $"{SureSonuTarihi:dd.MM.yyyy}";
        public string BeyannameTarihiDisplay => $"{BeyannameTarihi:dd.MM.yyyy}";

        public List<ChepStokGirisDetayDTO> ChepStokGirisDetayList { get; set; }
        public List<int> DeletedChepStokGirisDetayIdList { get; set; }
    }
}