using System;

namespace SparksX.Data.Models
{
    public partial class SentMail
    {
        public int SentMailId { get; set; }
        public int MailReportId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string EmailAddress { get; set; }
        public DateTime SentDate { get; set; }

        public virtual MailReport MailReport { get; set; }
    }
}