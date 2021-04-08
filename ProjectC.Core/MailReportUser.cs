using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ProjectC.Core
{
    public partial class MailReportUser
    {
        public int MailReportUserId { get; set; }
        public int MailReportId { get; set; }
        public int MailDefinitionId { get; set; }
        public byte ReceiverTypeId { get; set; }

        public virtual MailDefinition MailDefinition { get; set; }
        public virtual MailReport MailReport { get; set; }
    }
}
