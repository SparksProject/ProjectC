using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ProjectC.Core
{
    public partial class SentMail
    {
        public int SentMailId { get; set; }
        public int MailReportId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string EmailAddress { get; set; }
        public DateTime SentDate { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public string EmailCc { get; set; }

        public virtual MailReport MailReport { get; set; }
    }
}
