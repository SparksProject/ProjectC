using System;
using System.Collections.Generic;
using System.Linq;

namespace SparksX.DTO
{
    public class UserDTO
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Company { get; set; }
        public string Branch { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string EmailAddress { get; set; }
        public string PowerBI { get; set; }
        public byte UserTypeId { get; set; }
        public byte RecordStatusId { get; set; }
        public DateTime CreatedDate { get; set; }
        public int? CreatedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime? DeletedDate { get; set; }
        public int? DeletedBy { get; set; }

        public Guid CompanyId { get; set; }
        public List<int> ModuleInteractionList { get; set; }
        public List<UserCustomerDTO> UserCustomerList { get; set; }
        public string CustomerNames { get { return UserCustomerList != null ? string.Join(",", UserCustomerList.Select(x => x.CustomerName)) : ""; } }
        public List<Guid> CustomerIdList { get; set; }
        public string Token { get; set; }
        public string UserTypeName { get; set; }
        public string RecordStatusName { get; set; }
        public string CreatedByName { get; set; }
        public string ModifiedByName { get; set; }
        public string DeletedByName { get; set; }
        public Guid CustomerId { get; set; }
        public UserPermissionDTO UserPermissions { get; set; }

        // Only GET
        public string CreatedDateDisplay { get { return CreatedDate.ToString("dd/MM/yyyy HH:mm"); } }
        public string ModifiedDateDisplay { get { return ModifiedDate?.ToString("dd/MM/yyyy HH:mm"); } }
        public string DeletedDateDisplay { get { return DeletedDate?.ToString("dd/MM/yyyy HH:mm"); } }
        public string FullName { get { return $"{FirstName} {LastName}"; } }
    }
}