using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
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
        public string DosyaNo { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual User ModifiedByNavigation { get; set; }
        public virtual RecordStatus RecordStatus { get; set; }
        public virtual ICollection<Invoice> Invoice { get; set; }
    }
}
