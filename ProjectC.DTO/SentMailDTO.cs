using System;

namespace Chep.DTO
{
    public class SentMailDTO
    {
        public int SentMailId { get; set; }
        public int MailReportId { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string EmailAddress { get; set; }
        public DateTime SentDate { get; set; }
    }
}
