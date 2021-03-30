using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ProjectC.Data.Models
{
    public partial class SparksArchive
    {
        [Key]
        public Guid ArchiveId { get; set; }
        public Guid CustomerId { get; set; }
        public string DosyaTipi { get; set; }
        public string DosyaNo { get; set; }
        public string BelgeAdi { get; set; }
        public string Gosterme { get; set; }
        public DateTime InsDate { get; set; }
        public virtual Customer Customer { get; set; }

    }
}
