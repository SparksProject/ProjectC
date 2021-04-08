using System;

namespace Chep.DTO
{
    public class ArchiveFiltersDTO
    {
        public int UserId { get; set; }
        public string TescilNo { get; set; }
        public string DosyaNo { get; set; }
        public string FaturaNo { get; set; }
        public DateTime? TescilTarihiBaslangic { get; set; }
        public DateTime? TescilTarihiBitis { get; set; }
    }
}