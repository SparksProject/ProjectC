using System;
using System.Collections.Generic;

namespace ProjectC.Data.Models
{
    public partial class User
    {
        public User()
        {
            CompanyCreatedByNavigation = new HashSet<Company>();
            CompanyDeletedByNavigation = new HashSet<Company>();
            CompanyModifiedByNavigation = new HashSet<Company>();
            CustomerCreatedByNavigation = new HashSet<Customer>();
            CustomerDeletedByNavigation = new HashSet<Customer>();
            CustomerModifiedByNavigation = new HashSet<Customer>();
            GenericReportUser = new HashSet<GenericReportUser>();
            InverseCreatedByNavigation = new HashSet<User>();
            InverseDeletedByNavigation = new HashSet<User>();
            InverseModifiedByNavigation = new HashSet<User>();
            ProductCreatedByNavigation = new HashSet<Product>();
            ProductDeletedByNavigation = new HashSet<Product>();
            ProductModifiedByNavigation = new HashSet<Product>();
            UserPermission = new HashSet<UserPermission>();
            WorkOrderMasterCreatedByNavigation = new HashSet<WorkOrderMaster>();
            WorkOrderMasterDeletedByNavigation = new HashSet<WorkOrderMaster>();
            WorkOrderMasterModifiedByNavigation = new HashSet<WorkOrderMaster>();
            UserCustomer = new HashSet<UserCustomer>();
        }

        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Branch { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }

        public string PowerBI { get; set; }

        public byte RecordStatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }
        public byte UserTypeId { get; set; }

        public virtual User CreatedByNavigation { get; set; }
        public virtual User DeletedByNavigation { get; set; }
        public virtual User ModifiedByNavigation { get; set; }
        public virtual RecordStatus RecordStatus { get; set; }
        public virtual UserType UserType { get; set; }
        public virtual ICollection<Company> CompanyCreatedByNavigation { get; set; }
        public virtual ICollection<Company> CompanyDeletedByNavigation { get; set; }
        public virtual ICollection<Company> CompanyModifiedByNavigation { get; set; }
        public virtual ICollection<Customer> CustomerCreatedByNavigation { get; set; }
        public virtual ICollection<Customer> CustomerDeletedByNavigation { get; set; }
        public virtual ICollection<Customer> CustomerModifiedByNavigation { get; set; }
        public virtual ICollection<GenericReportUser> GenericReportUser { get; set; }
        public virtual ICollection<User> InverseCreatedByNavigation { get; set; }
        public virtual ICollection<User> InverseDeletedByNavigation { get; set; }
        public virtual ICollection<User> InverseModifiedByNavigation { get; set; }
        public virtual ICollection<Product> ProductCreatedByNavigation { get; set; }
        public virtual ICollection<Product> ProductDeletedByNavigation { get; set; }
        public virtual ICollection<Product> ProductModifiedByNavigation { get; set; }
        public virtual ICollection<UserPermission> UserPermission { get; set; }
        public virtual ICollection<WorkOrderMaster> WorkOrderMasterCreatedByNavigation { get; set; }
        public virtual ICollection<WorkOrderMaster> WorkOrderMasterDeletedByNavigation { get; set; }
        public virtual ICollection<WorkOrderMaster> WorkOrderMasterModifiedByNavigation { get; set; }
        public virtual ICollection<UserCustomer> UserCustomer { get; set; }
    }
}