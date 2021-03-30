using System;
using System.Collections.Generic;

namespace SparksX.Data.Models
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