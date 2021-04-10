using System;
using System.Collections.Generic;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
{
    public partial class Customer
    {
        public Customer()
        {
            ChepStokCikis = new HashSet<ChepStokCikis>();
            ChepStokGirisIhracatciFirmaNavigation = new HashSet<ChepStokGiris>();
            ChepStokGirisIthalatciFirmaNavigation = new HashSet<ChepStokGiris>();
            Product = new HashSet<Product>();
            SparksArchive = new HashSet<SparksArchive>();
            UserCustomer = new HashSet<UserCustomer>();
            WorkOrderMaster = new HashSet<WorkOrderMaster>();
        }

        public Guid CustomerId { get; set; }
        public string Name { get; set; }
        public string TaxNo { get; set; }
        public string TaxName { get; set; }
        public string Adress { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string Telephone { get; set; }
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
        public int? MailPeriodType { get; set; }
        public int? MailTime { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual User ModifiedByNavigation { get; set; }
        public virtual RecordStatus RecordStatus { get; set; }
        public virtual ICollection<ChepStokCikis> ChepStokCikis { get; set; }
        public virtual ICollection<ChepStokGiris> ChepStokGirisIhracatciFirmaNavigation { get; set; }
        public virtual ICollection<ChepStokGiris> ChepStokGirisIthalatciFirmaNavigation { get; set; }
        public virtual ICollection<Product> Product { get; set; }
        public virtual ICollection<SparksArchive> SparksArchive { get; set; }
        public virtual ICollection<UserCustomer> UserCustomer { get; set; }
        public virtual ICollection<WorkOrderMaster> WorkOrderMaster { get; set; }
    }
}
