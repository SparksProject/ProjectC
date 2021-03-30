using System;

namespace SparksX.DTO
{
    public class SparksArchiveDTO
    {
        public Guid ArchiveId { get; set; }
        public Guid CustomerId { get; set; }
        public string DosyaTipi { get; set; }
        public string DosyaNo { get; set; }
        public string BelgeAdi { get; set; }
        public string Gosterme { get; set; }        
        public string CustomerName { get; set; }
        public DateTime InsDate { get; set; }
    }
}