using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
{
    public partial class GenericReportParameter
    {
        public int GenericReportParameterId { get; set; }
        public int GenericReportId { get; set; }
        public string GenericReportParameterName { get; set; }
        public string ParameterLabel { get; set; }
        public short ParameterType { get; set; }

        public virtual GenericReport GenericReport { get; set; }
    }
}
