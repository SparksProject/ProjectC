using Microsoft.EntityFrameworkCore.Storage;
using Chep.Core;
using System;

namespace Chep.Data.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private ChepContext _context;
        private IDbContextTransaction transaction;

        public UnitOfWork()
        {
            _context = new ChepContext();
        }

        public bool Commit()
        {
            transaction = _context.Database.BeginTransaction();
            var result = _context.SaveChanges();
            transaction.Commit();
            return result > 0;
        }

        public void RollBack()
        {
            transaction.Rollback();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }

        public Repository<Company> Companies { get { return new Repository<Company>(_context); } }
        public Repository<Customer> Customers { get { return new Repository<Customer>(_context); } }
        public Repository<ExceptionLog> ExceptionLogs { get { return new Repository<ExceptionLog>(_context); } }
        public Repository<GenericReport> GenericReports { get { return new Repository<GenericReport>(_context); } }
        public Repository<GenericReportParameter> GenericReportParameters { get { return new Repository<GenericReportParameter>(_context); } }        
        public Repository<Product> Products { get { return new Repository<Product>(_context); } }
        public Repository<RecordStatus> RecordStatuses { get { return new Repository<RecordStatus>(_context); } }
        public Repository<SentMail> SentMails { get { return new Repository<SentMail>(_context); } }
        public Repository<User> Users { get { return new Repository<User>(_context); } }
        public Repository<UserPermission> UserPermissions { get { return new Repository<UserPermission>(_context); } }        
        public Repository<MailDefinition> MailDefinitions { get { return new Repository<MailDefinition>(_context); } }
        public Repository<MailReport> MailReports { get { return new Repository<MailReport>(_context); } }
        public Repository<MailReportUser> MailReportUsers { get { return new Repository<MailReportUser>(_context); } }
        public Repository<PeriodType> PeriodTypes { get { return new Repository<PeriodType>(_context); } }
        public Repository<GenericReportUser> GenericReportUsers { get { return new Repository<GenericReportUser>(_context); } }
        public Repository<UserType> UserTypes { get { return new Repository<UserType>(_context); } }       
        public Repository<SparksArchive> SparksArchives { get { return new Repository<SparksArchive>(_context); } }
        public Repository<UserCustomer> UserCustomers { get { return new Repository<UserCustomer>(_context); } }
        public Repository<Customs> Customs { get { return new Repository<Customs>(_context); } }
        public Repository<Units> Units { get { return new Repository<Units>(_context); } }
        public Repository<Country> Country { get { return new Repository<Country>(_context); } }
        public Repository<CurrencyType> CurrencyType { get { return new Repository<CurrencyType>(_context); } }
        public Repository<DeliveryTerms> DeliveryTerms { get { return new Repository<DeliveryTerms>(_context); } }
        public Repository<PaymentMethod> PaymentMethod { get { return new Repository<PaymentMethod>(_context); } }

        // chep
        public Repository<ChepStokGiris> ChepStokGiris { get { return new Repository<ChepStokGiris>(_context); } }
        public Repository<ChepStokGirisDetay> ChepStokGirisDetay { get { return new Repository<ChepStokGirisDetay>(_context); } }
        public Repository<ChepStokCikis> ChepStokCikis { get { return new Repository<ChepStokCikis>(_context); } }
        public Repository<ChepStokCikisDetay> ChepStokCikisDetay { get { return new Repository<ChepStokCikisDetay>(_context); } }

        // views
        public Repository<VwStokDusumListe> ViewStokDusumListe { get { return new Repository<VwStokDusumListe>(_context); } }
        public Repository<VwWorkOrderMaster> VwWorkOrderMaster { get { return new Repository<VwWorkOrderMaster>(_context); } }
        public Repository<VwWorkOrderInvoiceDetails> VwWorkOrderInvoiceDetails { get { return new Repository<VwWorkOrderInvoiceDetails>(_context); } }
        public Repository<VwWorkOrderInvoice> VwWorkOrderInvoice { get { return new Repository<VwWorkOrderInvoice>(_context); } }
    }
}