using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace ProjectC.Core
{
    public partial class GenericReport
    {
        public GenericReport()
        {
            GenericReportParameter = new HashSet<GenericReportParameter>();
            GenericReportUser = new HashSet<GenericReportUser>();
        }

        public int GenericReportId { get; set; }
        public string GenericReportName { get; set; }
        public string SqlScript { get; set; }
        public bool IsDefaultReport { get; set; }
        public byte RecordStatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }

        public virtual RecordStatus RecordStatus { get; set; }
        public virtual ICollection<GenericReportParameter> GenericReportParameter { get; set; }
        public virtual ICollection<GenericReportUser> GenericReportUser { get; set; }
    }
}
