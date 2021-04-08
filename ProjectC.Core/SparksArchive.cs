using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ProjectC.Core
{
    public partial class SparksArchive
    {
        public Guid ArchiveId { get; set; }
        public string DosyaTipi { get; set; }
        public string DosyaNo { get; set; }
        public Guid CustomerId { get; set; }
        public string BelgeAdi { get; set; }
        public string DosyaYolu { get; set; }
        public bool Gosterme { get; set; }
        public DateTime InsDate { get; set; }
        public string FileDate { get; set; }

        public virtual Customer Customer { get; set; }
    }
}
