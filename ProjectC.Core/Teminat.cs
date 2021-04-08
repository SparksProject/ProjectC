using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ProjectC.Core
{
    public partial class Teminat
    {
        public int TeminatId { get; set; }
        public int TeminatTipiId { get; set; }
        public int DurumId { get; set; }
        public string DosyaTipi { get; set; }
        public string DosyaNo { get; set; }
        public string Gonderici { get; set; }
        public string Alici { get; set; }
        public string TescilNo { get; set; }
        public DateTime? TescilTarihi { get; set; }
        public string Gumruk { get; set; }
        public string Banka { get; set; }
        public string AntrepoKodu { get; set; }
        public string TeminatRefNo { get; set; }
        public decimal? TeminatTutari { get; set; }
        public decimal? OdenecekTutar { get; set; }
        public string MuracatNo { get; set; }
        public DateTime? MuracatTarihi { get; set; }
        public DateTime? CozumTarihi { get; set; }
        public string Aciklama { get; set; }
        public string EvrakTeslimAlan { get; set; }
        public DateTime? EvrakTeslimAlmaTarihi { get; set; }
        public string EvrakTeslimEden { get; set; }
        public DateTime? EvrakTeslimTarihi { get; set; }
        public DateTime? InsDate { get; set; }
    }
}
