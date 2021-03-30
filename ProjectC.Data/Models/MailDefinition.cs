using System;
using System.Collections.Generic;

namespace ProjectC.Data.Models
{
    public partial class MailDefinition
    {
        public MailDefinition()
        {
            MailReportUser = new HashSet<MailReportUser>();
        }

        public int MailDefinitionId { get; set; }
        public string RecipientName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime CreatedDate { get; set; }

        public virtual ICollection<MailReportUser> MailReportUser { get; set; }
    }
}
