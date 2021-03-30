using System;
using System.ComponentModel.DataAnnotations;

namespace ProjectC.Data.Models
{
    public class Arsiv
    {
        [Key]

        public int Id { get; set; }
        public string DosyaNo { get; set; }
        public string Dosya { get; set; }
        public int FirmaNo { get; set; }
        public string Firma { get; set; }
        public string Alici { get; set; }
        public string MusRefNo { get; set; }
        public string FaturaNo { get; set; }
        public string TescilNo { get; set; }

        [DisplayFormat(DataFormatString = "{yyyy/MM/dd}")]
        public DateTime TescilTarihi { get; set; }
        public string ArsivPath { get; set; }
    }
}
