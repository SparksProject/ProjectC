using System;
using System.Collections.Generic;

namespace Chep.DTO
{
    public class ChepStokGirisDetayDTO
    {
        public int StokGirisDetayId { get; set; }
        public int StokGirisId { get; set; }
        public int? TpsSiraNo { get; set; }
        public int? TpsCikisSiraNo { get; set; }
        public int? BeyannameKalemNo { get; set; }
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
        public string BeyannameNo { get; set; }
        public DateTime? BeyannameTarihi { get; set; }


        public string StokGirisBeyannameNo { get; set; }
        public string TpsNo { get; set; }
        public List<ChepStokCikisDetayDTO> ChepStokCikisDetayList { get; set; }

        public string FaturaTarihDisplay => FaturaTarih == null ? null : $"{FaturaTarih.Value:dd.MM.yyyy}";
        public string FaturaTutarDisplay => $"{FaturaTutar:N2}";
    }
}