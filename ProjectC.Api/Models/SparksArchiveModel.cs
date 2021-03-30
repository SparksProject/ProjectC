using System;

namespace ProjectC.Api.Models
{
    public class SparksArchiveModel
    {
        public Guid CustomerId { get; set; }
        public string File { get; set; }
        public string DosyaTipi { get; set; }
        public string DosyaNo { get; set; }
        public string DosyaAdi { get; set; }
        public string Gosterme { get; set; }
    }
}
