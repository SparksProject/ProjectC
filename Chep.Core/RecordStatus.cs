using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
{
    public partial class RecordStatus
    {
        public RecordStatus()
        {
            Company = new HashSet<Company>();
            Customer = new HashSet<Customer>();
            GenericReport = new HashSet<GenericReport>();
            MailReport = new HashSet<MailReport>();
            Product = new HashSet<Product>();
            User = new HashSet<User>();
            WorkOrderMaster = new HashSet<WorkOrderMaster>();
        }

        public byte RecordStatusId { get; set; }
        public string RecordStatusName { get; set; }

        public virtual ICollection<Company> Company { get; set; }
        public virtual ICollection<Customer> Customer { get; set; }
        public virtual ICollection<GenericReport> GenericReport { get; set; }
        public virtual ICollection<MailReport> MailReport { get; set; }
        public virtual ICollection<Product> Product { get; set; }
        public virtual ICollection<User> User { get; set; }
        public virtual ICollection<WorkOrderMaster> WorkOrderMaster { get; set; }
    }
}
