using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
{
    public partial class Company
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Guid ProjectId { get; set; }
        public string SmtpHost { get; set; }
        public int? SmtpPort { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set; }
        public bool? SmtpIsSslenabled { get; set; }
        public byte RecordStatusId { get; set; }
        public string ProductEmail { get; set; }
        public string ArchivePath { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual User ModifiedByNavigation { get; set; }
        public virtual RecordStatus RecordStatus { get; set; }
    }
}
