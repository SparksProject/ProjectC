using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
{
    public partial class GenericReportUser
    {
        public int GenericReportUserId { get; set; }
        public int GenericReportId { get; set; }
        public int UserId { get; set; }

        public virtual GenericReport GenericReport { get; set; }
        public virtual User User { get; set; }
    }
}
