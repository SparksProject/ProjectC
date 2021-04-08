using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ProjectC.Core
{
    public partial class MailReport
    {
        public MailReport()
        {
            MailReportUser = new HashSet<MailReportUser>();
            SentMail = new HashSet<SentMail>();
        }

        public int MailReportId { get; set; }
        public string MailReportName { get; set; }
        public string Subject { get; set; }
        public string BodyTemplate { get; set; }
        public string SqlScript { get; set; }
        public int PeriodTypeId { get; set; }
        public string PeriodValue { get; set; }
        public byte RecordStatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
        public string PeriodDay { get; set; }

        public virtual PeriodType PeriodType { get; set; }
        public virtual RecordStatus RecordStatus { get; set; }
        public virtual ICollection<MailReportUser> MailReportUser { get; set; }
        public virtual ICollection<SentMail> SentMail { get; set; }
    }
}
