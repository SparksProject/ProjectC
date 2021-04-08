using ProjectC.Core;

using System;

namespace ProjectC.Data.Repository
{
    public interface IUnitOfWork : IDisposable
    {
        bool Commit();
        void RollBack();

        Repository<Company> Companies { get; }
        Repository<Customer> Customers { get; }
        Repository<ExceptionLog> ExceptionLogs { get; }
        Repository<GenericReport> GenericReports { get; }
        Repository<GenericReportParameter> GenericReportParameters { get; }
        Repository<Invoice> Invoices { get; }
        Repository<InvoiceDetail> InvoiceDetails { get; }
        Repository<MailDefinition> MailDefinitions { get; }
        Repository<MailReport> MailReports { get; }
        Repository<MailReportUser> MailReportUsers { get; }
        Repository<PeriodType> PeriodTypes { get; }
        Repository<Product> Products { get; }
        Repository<RecordStatus> RecordStatuses { get; }
        Repository<SentMail> SentMails { get; }
        Repository<User> Users { get; }
        Repository<UserPermission> UserPermissions { get; }
        Repository<UserCustomer> UserCustomers { get; }
        Repository<WorkOrderMaster> WorkOrderMasters { get; }
        Repository<GenericReportUser> GenericReportUsers { get; }
        Repository<UserType> UserTypes { get; }
        Repository<Teminat> Teminats { get; }
        Repository<SparksArchive> SparksArchives { get; }

        Repository<ChepStokGiris> ChepStokGiris { get; }
        Repository<ChepStokGirisDetay> ChepStokGirisDetay { get; }
        Repository<ChepStokCikis> ChepStokCikis { get; }
        Repository<ChepStokCikisDetay> ChepStokCikisDetay { get; }

        // Views
        Repository<VwStokDusumListe> ViewStokDusumListe { get; }
    }
}
