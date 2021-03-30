using System;
using System.Collections.Generic;

namespace ProjectC.Data.Models
{
    public partial class Customer
    {
        public Customer()
        {
            Product = new HashSet<Product>();
            SparksArchive = new HashSet<SparksArchive>();
            WorkOrderMaster = new HashSet<WorkOrderMaster>();
        }

        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string OtherId { get; set; }
        public string UserNameWs { get; set; }
        public string PasswordWs { get; set; }
        public byte RecordStatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual User ModifiedByNavigation { get; set; }
        public virtual RecordStatus RecordStatus { get; set; }
        public virtual ICollection<Product> Product { get; set; }
        public virtual ICollection<SparksArchive> SparksArchive { get; set; }
        public virtual ICollection<WorkOrderMaster> WorkOrderMaster { get; set; }
    }
}