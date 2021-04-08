using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
{
    public partial class PeriodType
    {
        public PeriodType()
        {
            MailReport = new HashSet<MailReport>();
        }

        public int PeriodTypeId { get; set; }
        public string PeriodTypeName { get; set; }

        public virtual ICollection<MailReport> MailReport { get; set; }
    }
}
