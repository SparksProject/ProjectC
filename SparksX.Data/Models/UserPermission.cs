using System;

namespace SparksX.Data.Models
{
    public class UserPermission
    {
        public UserPermission()
        {
        }

        public int UserPermissionId { get; set; }
        public int UserId { get; set; }
        public bool CompanyEdit { get; set; }
        public bool CustomerList { get; set; }
        public bool CustomerAdd { get; set; }
        public bool CustomerGet { get; set; }
        public bool CustomerEdit { get; set; }
        public bool GenericReportList { get; set; }
        public bool GenericReportAdd { get; set; }
        public bool GenericReportGet { get; set; }
        public bool GenericReportEdit { get; set; }
        public bool GenericReportExecute { get; set; }
        public bool MailDefinitionList { get; set; }
        public bool MailDefinitionAdd { get; set; }
        public bool MailDefinitionGert { get; set; }
        public bool MailDefinitionEdit { get; set; }
        public bool MailReportList { get; set; }
        public bool MailReportAdd { get; set; }
        public bool MailReportGet { get; set; }
        public bool MailReportEdit { get; set; }
        public bool MailReportExecute { get; set; }
        public bool WorkOrderMasterList { get; set; }
        public bool WorkOrderMasterAdd { get; set; }
        public bool WorkOrderMasterGet { get; set; }
        public bool WorkOrderMasterEdit { get; set; }
        public bool ProductList { get; set; }
        public bool ProductAdd { get; set; }
        public bool ProductGet { get; set; }
        public bool ProductEdit { get; set; }
        public bool UserList { get; set; }
        public bool UserAdd { get; set; }
        public bool UserGet { get; set; }
        public bool UserEdit { get; set; }
        public bool SparksArchiveList { get; set; }
        public bool EvrimArchiveList { get; set; }
        public bool StokGirisList { get; set; }
        public bool StokGirisAdd { get; set; }
        public bool StokGirisGet { get; set; }
        public bool StokGirisEdit { get; set; }
        public bool StokCikisList { get; set; }
        public bool StokCikisAdd { get; set; }
        public bool StokCikisGet { get; set; }
        public bool StokCikisEdit { get; set; }

        public bool BeyannameList { get; set; }
        public bool SparksArchiveImport { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public virtual User User { get; set; }
    }
}