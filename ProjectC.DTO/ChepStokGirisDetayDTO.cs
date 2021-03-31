﻿using System;

namespace ProjectC.DTO
{
    public class ChepStokGirisDetayDTO
    {
        public int StokGirisDetayId { get; set; }
        public int StokGirisId { get; set; }
        public int TPSSiraNo { get; set; }
        public string TPSBeyan { get; set; }
        public string EsyaCinsi { get; set; }
        public string EsyaGTIP { get; set; }
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
        public string PO { get; set; }

        public string FaturaTarihDisplay
        {
            get
            {
                if (FaturaTarih.HasValue)
                {
                    return $"{FaturaTarih.Value:dd.MM.yyyy}";
                }

                return default;
            }
        }

        public string FaturaTutarDisplay => $"{FaturaTutar:N2}";
    }
}
