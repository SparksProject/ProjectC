using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
{
    public partial class ChepStokGirisDetay
    {
        public ChepStokGirisDetay()
        {
            ChepStokCikisDetay = new HashSet<ChepStokCikisDetay>();
        }

        public int StokGirisDetayId { get; set; }
        public int StokGirisId { get; set; }
        public int? TpsSiraNo { get; set; }
        public string TpsBeyan { get; set; }
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
        public string PoNo { get; set; }

        public virtual ChepStokGiris StokGiris { get; set; }
        public virtual ICollection<ChepStokCikisDetay> ChepStokCikisDetay { get; set; }
    }
}
