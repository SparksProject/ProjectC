using System.Collections.Generic;
using System.Net.Mail;

namespace ProjectC.DTO
{
    public class MailDTO
    {
        public string Subject { get; set; }
        public List<string> To { get; set; }
        public List<string> Cc { get; set; }
        public string Body { get; set; }

        public string Host { get; set; }
        public int? Port { get; set; }
        public string UserName { get; set; }
        public string Password{ get; set; }
        public Attachment Attachment { get; set; }
    }
}
