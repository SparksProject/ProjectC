using System;

namespace Chep.DTO
{
    public class CompanyDTO
    {
        public Guid CompanyId { get; set; }
        public string CompanyName { get; set; }
        public Guid ProjectId { get; set; }
        public string SmtpHost { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpUserName { get; set; }
        public string SmtpPassword { get; set; }
        public bool? SmtpIsSSLEnabled { get; set; }
        public byte RecordStatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }

        public string ArchivePath { get; set; }

        public string RecordStatusName { get; set; }
        public string CreatedDateDisplay { get { return CreatedDate.ToString("dd/MM/yyyy HH:mm"); } }
        public string CreatedByName { get; set; }
        public string ModifiedDateDisplay { get { return ModifiedDate?.ToString("dd/MM/yyyy HH:mm"); } }
        public string ModifiedByName { get; set; }
        public string DeletedDateDisplay { get { return DeletedDate?.ToString("dd/MM/yyyy HH:mm"); } }
        public string DeletedByName { get; set; }
    }
}
