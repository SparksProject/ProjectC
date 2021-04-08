using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ProjectC.Core
{
    public partial class VwStokGirisDetayListe
    {
        public int StokGirisId { get; set; }
        public string ReferansNo { get; set; }
        public string Tpsno { get; set; }
        public string Tpsdurum { get; set; }
        public DateTime? BasvuruTarihi { get; set; }
        public DateTime? SureSonuTarihi { get; set; }
        public string GumrukKod { get; set; }
        public string BeyannameNo { get; set; }
        public DateTime? BeyannameTarihi { get; set; }
        public string BelgeAd { get; set; }
        public string BelgeSart { get; set; }
        public string Tpsaciklama { get; set; }
        public int? IthalatciFirma { get; set; }
        public int? IhracatciFirma { get; set; }
        public int? KapAdet { get; set; }
        public int StokGirisDetayId { get; set; }
        public int? TpssiraNo { get; set; }
        public string Tpsbeyan { get; set; }
        public string EsyaCinsi { get; set; }
        public string EsyaGtip { get; set; }
        public string FaturaNo { get; set; }
        public DateTime? FaturaTarih { get; set; }
        public decimal? FaturaTutar { get; set; }
        public string FaturaDovizKod { get; set; }
        public int? Miktar { get; set; }
        public string OlcuBirimi { get; set; }
        public string Rejim { get; set; }
        public string CikisRejimi { get; set; }
        public string GidecegiUlke { get; set; }
        public string MenseUlke { get; set; }
        public string SozlesmeUlke { get; set; }
        public string Marka { get; set; }
        public string Model { get; set; }
        public string UrunKod { get; set; }
        public string Pono { get; set; }
        public int UserId { get; set; }
    }
}
