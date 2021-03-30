using System;
using System.Collections.Generic;

namespace ProjectC.Data.Models
{
    public partial class WorkOrderMaster
    {
        public WorkOrderMaster()
        {
            Invoice = new HashSet<Invoice>();
        }

        public Guid WorkOrderMasterId { get; set; }
        public string WorkOrderNo { get; set; }
        public string DeclarationType { get; set; }
        public int Status { get; set; }
        public Guid? MasterId { get; set; }
        public Guid CustomerId { get; set; }
        public string Message { get; set; }
        public byte RecordStatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual User ModifiedByNavigation { get; set; }
        public virtual RecordStatus RecordStatus { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
    }
}