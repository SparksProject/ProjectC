using System;

namespace ProjectC.DTO
{
    public class ViewSparksArchiveDTO
    {
        public Guid Id { get; set; }
        public Guid FirmaId { get; set; }
        public int UserId { get; set; }
        public string DosyaNo { get; set; }
        public string Firma { get; set; }
        public string Alici { get; set; }
        public string MusRefNo { get; set; }
        public string FaturaNo { get; set; }
        public string TescilNo { get; set; }
        public string ArsivPath { get; set; }
        public DateTime? TescilTarihi { get; set; }

        public string DosyaAdi { get; set; }
    }
}