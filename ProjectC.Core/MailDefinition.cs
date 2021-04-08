using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ProjectC.Core
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
