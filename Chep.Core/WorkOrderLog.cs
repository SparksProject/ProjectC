using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
{
    public partial class WorkOrderLog
    {
        public Guid WorkOrderLogId { get; set; }
        public string WorkOrderNo { get; set; }
        public string Error { get; set; }
        public DateTime InsDate { get; set; }
    }
}
