using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
{
    public partial class ChepContext : DbContext
    {
        public ChepContext()
        {
        }

        public ChepContext(DbContextOptions<ChepContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChepStokCikis> ChepStokCikis { get; set; }
        public virtual DbSet<ChepStokCikisDetay> ChepStokCikisDetay { get; set; }
        public virtual DbSet<ChepStokGiris> ChepStokGiris { get; set; }
        public virtual DbSet<ChepStokGirisDetay> ChepStokGirisDetay { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<CurrencyType> CurrencyType { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Customs> Customs { get; set; }
        public virtual DbSet<DeliveryTerms> DeliveryTerms { get; set; }
        public virtual DbSet<ExceptionLog> ExceptionLog { get; set; }
        public virtual DbSet<GenericReport> GenericReport { get; set; }
        public virtual DbSet<GenericReportParameter> GenericReportParameter { get; set; }
        public virtual DbSet<GenericReportUser> GenericReportUser { get; set; }
        public virtual DbSet<MailDefinition> MailDefinition { get; set; }
        public virtual DbSet<MailReport> MailReport { get; set; }
        public virtual DbSet<MailReportUser> MailReportUser { get; set; }
        public virtual DbSet<PaymentMethod> PaymentMethod { get; set; }
        public virtual DbSet<PeriodType> PeriodType { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<RecordStatus> RecordStatus { get; set; }
        public virtual DbSet<SentMail> SentMail { get; set; }
        public virtual DbSet<Units> Units { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserCustomer> UserCustomer { get; set; }
        public virtual DbSet<UserPermission> UserPermission { get; set; }
        public virtual DbSet<UserType> UserType { get; set; }
        public virtual DbSet<VwGenelListe> VwGenelListe { get; set; }
        public virtual DbSet<VwStokCikisDetayListe> VwStokCikisDetayListe { get; set; }
        public virtual DbSet<VwStokCikisFaturaOrnekListe> VwStokCikisFaturaOrnekListe { get; set; }
        public virtual DbSet<VwStokCikisFordListe> VwStokCikisFordListe { get; set; }
        public virtual DbSet<VwStokDurumListe> VwStokDurumListe { get; set; }
        public virtual DbSet<VwStokDusumListe> VwStokDusumListe { get; set; }
        public virtual DbSet<VwStokGirisDetayListe> VwStokGirisDetayListe { get; set; }
        public virtual DbSet<VwSureTakipListe> VwSureTakipListe { get; set; }
        public virtual DbSet<VwWsWorkOrderInvoice> VwWsWorkOrderInvoice { get; set; }
        public virtual DbSet<VwWsWorkOrderInvoiceDetails> VwWsWorkOrderInvoiceDetails { get; set; }
        public virtual DbSet<VwWsWorkOrderInvoiceDetailsTcgb> VwWsWorkOrderInvoiceDetailsTcgb { get; set; }
        public virtual DbSet<VwWsWorkOrderMaster> VwWsWorkOrderMaster { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("User ID=admin;Password=chep2021;Server=database-1.c7nonlbizeql.us-east-2.rds.amazonaws.com,1433;Database=Chep;Pooling=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ChepStokCikis>(entity =>
            {
                entity.HasKey(e => e.StokCikisId)
                    .HasName("PK__ChepStok__FDAB99CF950106B1");

                entity.Property(e => e.BeyannameNo).HasMaxLength(20);

                entity.Property(e => e.BeyannameTarihi).HasColumnType("datetime");

                entity.Property(e => e.CikisAracKimligi).HasMaxLength(35);

                entity.Property(e => e.CikisGumruk).HasMaxLength(6);

                entity.Property(e => e.GtbReferenceNo).HasMaxLength(25);

                entity.Property(e => e.InvoiceAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InvoiceCurrency).HasMaxLength(3);

                entity.Property(e => e.InvoiceDate).HasColumnType("date");

                entity.Property(e => e.InvoiceNo).HasMaxLength(50);

                entity.Property(e => e.IsEmriDurum).HasMaxLength(300);

                entity.Property(e => e.IslemTarihi).HasColumnType("datetime");

                entity.Property(e => e.KapCinsi).HasMaxLength(2);

                entity.Property(e => e.OdemeSekli).HasMaxLength(2);

                entity.Property(e => e.TeslimSekli).HasMaxLength(3);

                entity.Property(e => e.TpsNo).HasMaxLength(50);

                entity.Property(e => e.TpsTarih).HasColumnType("datetime");

                entity.HasOne(d => d.IhracatciFirmaNavigation)
                    .WithMany(p => p.ChepStokCikis)
                    .HasForeignKey(d => d.IhracatciFirma)
                    .HasConstraintName("FK_ChepStokCikis_Customer");
            });

            modelBuilder.Entity<ChepStokCikisDetay>(entity =>
            {
                entity.HasKey(e => e.StokCikisDetayId)
                    .HasName("PK__ChepStok__7A5C97E41CA2F026");

                entity.Property(e => e.BirimTutar).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.BrutKg).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InvoiceAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.NetKg).HasColumnType("decimal(18, 2)");

                entity.HasOne(d => d.StokCikis)
                    .WithMany(p => p.ChepStokCikisDetay)
                    .HasForeignKey(d => d.StokCikisId)
                    .HasConstraintName("FK_ChepStokCikisDetay_ChepStokCikis");

                entity.HasOne(d => d.StokGirisDetay)
                    .WithMany(p => p.ChepStokCikisDetay)
                    .HasForeignKey(d => d.StokGirisDetayId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ChepStokCikisDetay_ChepStokGirisDetay");
            });

            modelBuilder.Entity<ChepStokGiris>(entity =>
            {
                entity.HasKey(e => e.StokGirisId)
                    .HasName("PK__ChepStok__41A2AA5533234034");

                entity.Property(e => e.AktarimTarihi).HasColumnType("date");

                entity.Property(e => e.BasvuruTarihi).HasColumnType("datetime");

                entity.Property(e => e.BelgeAd).HasMaxLength(50);

                entity.Property(e => e.BelgeSart).HasMaxLength(50);

                entity.Property(e => e.BeyannameNo).HasMaxLength(16);

                entity.Property(e => e.BeyannameTarihi).HasColumnType("datetime");

                entity.Property(e => e.GumrukKod).HasMaxLength(6);

                entity.Property(e => e.SureSonuTarihi).HasColumnType("datetime");

                entity.Property(e => e.TpsAciklama).HasMaxLength(250);

                entity.Property(e => e.TpsDurum).HasMaxLength(50);

                entity.Property(e => e.TpsNo)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasOne(d => d.IhracatciFirmaNavigation)
                    .WithMany(p => p.ChepStokGirisIhracatciFirmaNavigation)
                    .HasForeignKey(d => d.IhracatciFirma)
                    .HasConstraintName("FK_ChepStokGiris_Customer1");

                entity.HasOne(d => d.IthalatciFirmaNavigation)
                    .WithMany(p => p.ChepStokGirisIthalatciFirmaNavigation)
                    .HasForeignKey(d => d.IthalatciFirma)
                    .HasConstraintName("FK_ChepStokGiris_Customer");
            });

            modelBuilder.Entity<ChepStokGirisDetay>(entity =>
            {
                entity.HasKey(e => e.StokGirisDetayId)
                    .HasName("PK__ChepStok__5F1F65B0F75ADB99");

                entity.Property(e => e.BeyannameNo).HasMaxLength(16);

                entity.Property(e => e.BeyannameTarihi).HasColumnType("datetime");

                entity.Property(e => e.CikisRejimi).HasMaxLength(50);

                entity.Property(e => e.EsyaCinsi).HasMaxLength(20);

                entity.Property(e => e.EsyaGtip).HasMaxLength(12);

                entity.Property(e => e.FaturaDovizKod).HasMaxLength(3);

                entity.Property(e => e.FaturaNo).HasMaxLength(30);

                entity.Property(e => e.FaturaTarih).HasColumnType("datetime");

                entity.Property(e => e.FaturaTutar).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.GidecegiUlke).HasMaxLength(20);

                entity.Property(e => e.Marka).HasMaxLength(50);

                entity.Property(e => e.MenseUlke).HasMaxLength(20);

                entity.Property(e => e.Model).HasMaxLength(50);

                entity.Property(e => e.OlcuBirimi).HasMaxLength(5);

                entity.Property(e => e.PoNo).HasMaxLength(50);

                entity.Property(e => e.Rejim).HasMaxLength(50);

                entity.Property(e => e.SozlesmeUlke).HasMaxLength(20);

                entity.Property(e => e.TpsBeyan).HasMaxLength(20);

                entity.Property(e => e.UrunKod).HasMaxLength(50);

                entity.HasOne(d => d.StokGiris)
                    .WithMany(p => p.ChepStokGirisDetay)
                    .HasForeignKey(d => d.StokGirisId)
                    .HasConstraintName("FK_ChepStokGirisDetay_ChepStokGiris");
            });

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.CompanyId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.ArchivePath)
                    .HasMaxLength(250)
                    .IsUnicode(false);

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ProductEmail)
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.SmtpHost).HasMaxLength(100);

                entity.Property(e => e.SmtpIsSslenabled).HasColumnName("SmtpIsSSLEnabled");

                entity.Property(e => e.SmtpPassword).HasMaxLength(50);

                entity.Property(e => e.SmtpUserName).HasMaxLength(100);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CompanyCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Company_Users");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CompanyDeletedByNavigation)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_Company_Users2");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.CompanyModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_Company_Users1");

                entity.HasOne(d => d.RecordStatus)
                    .WithMany(p => p.Company)
                    .HasForeignKey(d => d.RecordStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Company_RecordStatuses");
            });

            modelBuilder.Entity<Country>(entity =>
            {
                entity.Property(e => e.CountryName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.EdiCode)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.Property(e => e.IsoCode).HasMaxLength(2);
            });

            modelBuilder.Entity<CurrencyType>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.CurrencyTypeId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CurrencyTypeName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);

                entity.Property(e => e.EdiCode)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");
            });

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Adress).HasMaxLength(500);

                entity.Property(e => e.City).HasMaxLength(100);

                entity.Property(e => e.Country).HasMaxLength(100);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.OtherId).HasMaxLength(20);

                entity.Property(e => e.PasswordWs).HasMaxLength(20);

                entity.Property(e => e.RecordStatusId).HasDefaultValueSql("((1))");

                entity.Property(e => e.TaxName).HasMaxLength(100);

                entity.Property(e => e.TaxNo).HasMaxLength(50);

                entity.Property(e => e.Telephone).HasMaxLength(100);

                entity.Property(e => e.UserNameWs).HasMaxLength(20);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.CustomerCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Customer_Users");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.CustomerDeletedByNavigation)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_Customer_Users2");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.CustomerModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_Customer_Users1");

                entity.HasOne(d => d.RecordStatus)
                    .WithMany(p => p.Customer)
                    .HasForeignKey(d => d.RecordStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Customer_RecordStatuses");
            });

            modelBuilder.Entity<Customs>(entity =>
            {
                entity.Property(e => e.CustomsName)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.EdiCode)
                    .IsRequired()
                    .HasMaxLength(6);
            });

            modelBuilder.Entity<DeliveryTerms>(entity =>
            {
                entity.Property(e => e.EdiCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ExceptionLog>(entity =>
            {
                entity.Property(e => e.ExceptionDate).HasColumnType("datetime");
            });

            modelBuilder.Entity<GenericReport>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.GenericReportName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.SqlScript).IsRequired();

                entity.HasOne(d => d.RecordStatus)
                    .WithMany(p => p.GenericReport)
                    .HasForeignKey(d => d.RecordStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GenericReport_RecordStatus");
            });

            modelBuilder.Entity<GenericReportParameter>(entity =>
            {
                entity.Property(e => e.GenericReportParameterName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ParameterLabel)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.HasOne(d => d.GenericReport)
                    .WithMany(p => p.GenericReportParameter)
                    .HasForeignKey(d => d.GenericReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GenericReportParameter_GenericReport");
            });

            modelBuilder.Entity<GenericReportUser>(entity =>
            {
                entity.HasOne(d => d.GenericReport)
                    .WithMany(p => p.GenericReportUser)
                    .HasForeignKey(d => d.GenericReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GenericReportUser_GenericReport");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GenericReportUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_GenericReportUser_User");
            });

            modelBuilder.Entity<MailDefinition>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.RecipientName)
                    .IsRequired()
                    .HasMaxLength(100);
            });

            modelBuilder.Entity<MailReport>(entity =>
            {
                entity.Property(e => e.BodyTemplate).IsRequired();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.MailReportName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PeriodDay)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PeriodValue).HasMaxLength(100);

                entity.Property(e => e.SqlScript).IsRequired();

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.PeriodType)
                    .WithMany(p => p.MailReport)
                    .HasForeignKey(d => d.PeriodTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MailReport_PeriodType");

                entity.HasOne(d => d.RecordStatus)
                    .WithMany(p => p.MailReport)
                    .HasForeignKey(d => d.RecordStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MailReport_RecordStatus");
            });

            modelBuilder.Entity<MailReportUser>(entity =>
            {
                entity.HasOne(d => d.MailDefinition)
                    .WithMany(p => p.MailReportUser)
                    .HasForeignKey(d => d.MailDefinitionId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MailReportUser_MailDefinition");

                entity.HasOne(d => d.MailReport)
                    .WithMany(p => p.MailReportUser)
                    .HasForeignKey(d => d.MailReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_MailReportUser_MailReport");
            });

            modelBuilder.Entity<PaymentMethod>(entity =>
            {
                entity.Property(e => e.EdiCode)
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<PeriodType>(entity =>
            {
                entity.Property(e => e.PeriodTypeName)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).ValueGeneratedNever();

                entity.Property(e => e.CountryOfOrigin).HasMaxLength(3);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.CurrencyType).HasMaxLength(3);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.GrossWeight).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HsCode)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.NetWeight).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.ProductNameEng).HasMaxLength(100);

                entity.Property(e => e.ProductNameOrg).HasMaxLength(100);

                entity.Property(e => e.ProductNameTr)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ProductNo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.SapCode).HasMaxLength(50);

                entity.Property(e => e.UnitPrice).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Uom)
                    .HasMaxLength(3)
                    .IsFixedLength();

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.ProductCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Product_Users");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_Customer");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.ProductDeletedByNavigation)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_Product_Users2");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.ProductModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_Product_Users1");

                entity.HasOne(d => d.RecordStatus)
                    .WithMany(p => p.Product)
                    .HasForeignKey(d => d.RecordStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Product_RecordStatuses");
            });

            modelBuilder.Entity<RecordStatus>(entity =>
            {
                entity.Property(e => e.RecordStatusId).ValueGeneratedOnAdd();

                entity.Property(e => e.RecordStatusName)
                    .IsRequired()
                    .HasMaxLength(10);
            });

            modelBuilder.Entity<SentMail>(entity =>
            {
                entity.Property(e => e.Body)
                    .IsRequired()
                    .HasMaxLength(2000);

                entity.Property(e => e.CreatedDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("(getdate())");

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(150);

                entity.Property(e => e.EmailCc)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.SentDate).HasColumnType("datetime");

                entity.Property(e => e.Subject)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.HasOne(d => d.MailReport)
                    .WithMany(p => p.SentMail)
                    .HasForeignKey(d => d.MailReportId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SentMail_MailReport");
            });

            modelBuilder.Entity<Units>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.EdiCode)
                    .IsRequired()
                    .HasMaxLength(3)
                    .IsUnicode(false);

                entity.Property(e => e.Status)
                    .IsRequired()
                    .HasDefaultValueSql("((1))");

                entity.Property(e => e.UnitsId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.UnitsName)
                    .IsRequired()
                    .HasMaxLength(100)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.Property(e => e.Branch).HasMaxLength(150);

                entity.Property(e => e.Company).HasMaxLength(150);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.EmailAddress).HasMaxLength(100);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.PowerBi)
                    .HasColumnName("PowerBI")
                    .HasMaxLength(1000);

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.InverseCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_Users_Users");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.InverseDeletedByNavigation)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_Users_Users2");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.InverseModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_Users_Users1");

                entity.HasOne(d => d.RecordStatus)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.RecordStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_RecordStatus");

                entity.HasOne(d => d.UserType)
                    .WithMany(p => p.User)
                    .HasForeignKey(d => d.UserTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_User_UserTypeId");
            });

            modelBuilder.Entity<UserCustomer>(entity =>
            {
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.UserCustomer)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserCustomer_Customer");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserCustomer)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserCustomer_User");
            });

            modelBuilder.Entity<UserPermission>(entity =>
            {
                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.UserPermission)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_UserPermission_User");
            });

            modelBuilder.Entity<UserType>(entity =>
            {
                entity.Property(e => e.UserTypeId).ValueGeneratedOnAdd();

                entity.Property(e => e.UserTypeName).HasMaxLength(50);
            });

            modelBuilder.Entity<VwGenelListe>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_GenelListe");

                entity.Property(e => e.BasvuruTarihi).HasColumnType("datetime");

                entity.Property(e => e.BelgeAd).HasMaxLength(50);

                entity.Property(e => e.BelgeSart).HasMaxLength(50);

                entity.Property(e => e.CikisBeyannameNo).HasMaxLength(20);

                entity.Property(e => e.CikisBeyannameTarihi).HasColumnType("datetime");

                entity.Property(e => e.CikisIslemTarihi).HasColumnType("datetime");

                entity.Property(e => e.CikisRejimi).HasMaxLength(50);

                entity.Property(e => e.CikisTpsno)
                    .HasColumnName("CikisTPSNo")
                    .HasMaxLength(50);

                entity.Property(e => e.CikisTpstarih)
                    .HasColumnName("CikisTPSTarih")
                    .HasColumnType("datetime");

                entity.Property(e => e.EsyaCinsi).HasMaxLength(20);

                entity.Property(e => e.EsyaGtip).HasMaxLength(12);

                entity.Property(e => e.FaturaDovizKod).HasMaxLength(3);

                entity.Property(e => e.FaturaTutar).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.GidecegiUlke).HasMaxLength(20);

                entity.Property(e => e.GirisBeyannameNo).HasMaxLength(16);

                entity.Property(e => e.GirisBeyannameTarihi).HasColumnType("datetime");

                entity.Property(e => e.GirisFaturaNo).HasMaxLength(30);

                entity.Property(e => e.GirisFaturaTarih).HasColumnType("datetime");

                entity.Property(e => e.GirisTpsno)
                    .IsRequired()
                    .HasColumnName("GirisTPSNo")
                    .HasMaxLength(30);

                entity.Property(e => e.GirisTpssiraNo).HasColumnName("GirisTPSSiraNo");

                entity.Property(e => e.GumrukKod).HasMaxLength(6);

                entity.Property(e => e.Marka).HasMaxLength(50);

                entity.Property(e => e.MenseUlke).HasMaxLength(20);

                entity.Property(e => e.Model).HasMaxLength(50);

                entity.Property(e => e.OlcuBirimi).HasMaxLength(5);

                entity.Property(e => e.Pono)
                    .HasColumnName("PONo")
                    .HasMaxLength(50);

                entity.Property(e => e.Rejim).HasMaxLength(50);

                entity.Property(e => e.SozlesmeUlke).HasMaxLength(20);

                entity.Property(e => e.SureSonuTarihi).HasColumnType("datetime");

                entity.Property(e => e.Tpsaciklama)
                    .HasColumnName("TPSAciklama")
                    .HasMaxLength(250);

                entity.Property(e => e.Tpsbeyan)
                    .HasColumnName("TPSBeyan")
                    .HasMaxLength(20);

                entity.Property(e => e.Tpsdurum)
                    .HasColumnName("TPSDurum")
                    .HasMaxLength(50);

                entity.Property(e => e.UrunKod).HasMaxLength(50);
            });

            modelBuilder.Entity<VwStokCikisDetayListe>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_StokCikisDetayListe");

                entity.Property(e => e.BeyannameNo).HasMaxLength(16);

                entity.Property(e => e.BeyannameTarihi).HasColumnType("datetime");

                entity.Property(e => e.EsyaCinsi).HasMaxLength(20);

                entity.Property(e => e.EsyaGtip).HasMaxLength(12);

                entity.Property(e => e.IslemTarihi).HasColumnType("datetime");

                entity.Property(e => e.Marka).HasMaxLength(50);

                entity.Property(e => e.Model).HasMaxLength(50);

                entity.Property(e => e.Pono)
                    .HasColumnName("PONo")
                    .HasMaxLength(50);

                entity.Property(e => e.Tpsno)
                    .HasColumnName("TPSNo")
                    .HasMaxLength(50);

                entity.Property(e => e.Tpstarih)
                    .HasColumnName("TPSTarih")
                    .HasColumnType("datetime");

                entity.Property(e => e.UrunKod).HasMaxLength(50);
            });

            modelBuilder.Entity<VwStokCikisFaturaOrnekListe>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_StokCikisFaturaOrnekListe");

                entity.Property(e => e.AliciAdres)
                    .HasColumnName("Alici Adres")
                    .HasMaxLength(500);

                entity.Property(e => e.AlıcıFirma)
                    .HasColumnName("Alıcı Firma")
                    .HasMaxLength(100);

                entity.Property(e => e.BirimFiyat)
                    .HasColumnName("Birim Fiyat")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.BrütKg).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FaturaTutarı)
                    .HasColumnName("Fatura Tutarı")
                    .HasColumnType("decimal(18, 2)");

                entity.Property(e => e.GirişİthalatcıFirma)
                    .HasColumnName("Giriş İthalatcı Firma")
                    .HasMaxLength(100);

                entity.Property(e => e.Gtip)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.Gümrük).HasMaxLength(257);

                entity.Property(e => e.Menşei).HasMaxLength(20);

                entity.Property(e => e.NetKg).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.TeslimŞekli)
                    .HasColumnName("Teslim Şekli")
                    .HasMaxLength(3);

                entity.Property(e => e.TicariTanım)
                    .HasColumnName("Ticari Tanım")
                    .HasMaxLength(71);

                entity.Property(e => e.ÇıkışReferansNo).HasColumnName("Çıkış ReferansNo");

                entity.Property(e => e.ÜrünKodu)
                    .HasColumnName("Ürün Kodu")
                    .HasMaxLength(50);

                entity.Property(e => e.İhracatTpsNo)
                    .HasColumnName("İhracat TPS No")
                    .HasMaxLength(34);

                entity.Property(e => e.İhracatTpsTarihi)
                    .HasColumnName("İhracat TPS Tarihi")
                    .HasColumnType("datetime");

                entity.Property(e => e.İthalatBeyannameKalemNo).HasColumnName("İthalat Beyanname Kalem No");

                entity.Property(e => e.İthalatBeyannameNo)
                    .HasColumnName("İthalat Beyanname No")
                    .HasMaxLength(16);

                entity.Property(e => e.İthalatBeyannameTarihi)
                    .HasColumnName("İthalat Beyanname Tarihi")
                    .HasColumnType("datetime");
            });

            modelBuilder.Entity<VwStokCikisFordListe>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_StokCikisFordListe");

                entity.Property(e => e.Gtİp)
                    .HasColumnName("GTİP")
                    .HasMaxLength(12);

                entity.Property(e => e.Menşeİ)
                    .HasColumnName("MENŞEİ")
                    .HasMaxLength(20);

                entity.Property(e => e.ParçaAdedİ).HasColumnName("PARÇA ADEDİ");

                entity.Property(e => e.ParçaNo)
                    .HasColumnName("PARÇA NO")
                    .HasMaxLength(57);

                entity.Property(e => e.İhracatBeyannameNumarasi)
                    .HasColumnName("İHRACAT BEYANNAME NUMARASI")
                    .HasMaxLength(20);

                entity.Property(e => e.İhracatBeyannameTarİhİ)
                    .HasColumnName("İHRACAT BEYANNAME TARİHİ")
                    .HasMaxLength(4000);

                entity.Property(e => e.İhracatTpsNo)
                    .HasColumnName("İHRACAT TPS NO")
                    .HasMaxLength(34);

                entity.Property(e => e.İhracatTpsTarİhİ)
                    .HasColumnName("İHRACAT TPS TARİHİ")
                    .HasMaxLength(4000);

                entity.Property(e => e.İthalatBeyannameNo)
                    .HasColumnName("İTHALAT BEYANNAME NO")
                    .HasMaxLength(16);

                entity.Property(e => e.İthalatBeyannameTarİhİ)
                    .HasColumnName("İTHALAT BEYANNAME TARİHİ")
                    .HasMaxLength(4000);

                entity.Property(e => e.İthalİşlemTürü)
                    .HasColumnName("İTHAL İŞLEM TÜRÜ")
                    .HasMaxLength(2);
            });

            modelBuilder.Entity<VwStokDurumListe>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_StokDurumListe");

                entity.Property(e => e.BasvuruTarihi).HasColumnType("datetime");

                entity.Property(e => e.BelgeAd).HasMaxLength(50);

                entity.Property(e => e.BelgeSart).HasMaxLength(50);

                entity.Property(e => e.BeyannameNo).HasMaxLength(16);

                entity.Property(e => e.BeyannameTarihi).HasColumnType("datetime");

                entity.Property(e => e.CikisRejimi).HasMaxLength(50);

                entity.Property(e => e.EsyaCinsi).HasMaxLength(20);

                entity.Property(e => e.EsyaGtip).HasMaxLength(12);

                entity.Property(e => e.FaturaDovizKod).HasMaxLength(3);

                entity.Property(e => e.FaturaNo).HasMaxLength(30);

                entity.Property(e => e.FaturaTarih).HasColumnType("datetime");

                entity.Property(e => e.FaturaTutar).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.GidecegiUlke).HasMaxLength(20);

                entity.Property(e => e.GumrukKod).HasMaxLength(6);

                entity.Property(e => e.IhracatciFirma).HasMaxLength(100);

                entity.Property(e => e.IthalatciFirma).HasMaxLength(100);

                entity.Property(e => e.Marka).HasMaxLength(50);

                entity.Property(e => e.MenseUlke).HasMaxLength(20);

                entity.Property(e => e.Model).HasMaxLength(50);

                entity.Property(e => e.OlcuBirimi).HasMaxLength(5);

                entity.Property(e => e.Pono)
                    .HasColumnName("PONo")
                    .HasMaxLength(50);

                entity.Property(e => e.Rejim).HasMaxLength(50);

                entity.Property(e => e.SozlesmeUlke).HasMaxLength(20);

                entity.Property(e => e.SureSonuTarihi).HasColumnType("datetime");

                entity.Property(e => e.Tpsaciklama)
                    .HasColumnName("TPSAciklama")
                    .HasMaxLength(250);

                entity.Property(e => e.Tpsbeyan)
                    .HasColumnName("TPSBeyan")
                    .HasMaxLength(20);

                entity.Property(e => e.Tpsdurum)
                    .HasColumnName("TPSDurum")
                    .HasMaxLength(50);

                entity.Property(e => e.Tpsno)
                    .IsRequired()
                    .HasColumnName("TPSNo")
                    .HasMaxLength(30);

                entity.Property(e => e.TpssiraNo).HasColumnName("TPSSiraNo");

                entity.Property(e => e.UrunKod).HasMaxLength(50);
            });

            modelBuilder.Entity<VwStokDusumListe>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_StokDusumListe");

                entity.Property(e => e.BirimFiyat).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.BrutKg).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.GirisBeyannameNo).HasMaxLength(16);

                entity.Property(e => e.GirisBeyannameTarihi).HasColumnType("datetime");

                entity.Property(e => e.Marka).HasMaxLength(50);

                entity.Property(e => e.Model).HasMaxLength(50);

                entity.Property(e => e.NetKg).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.Pono)
                    .HasColumnName("PONo")
                    .HasMaxLength(50);

                entity.Property(e => e.SureSonuTarihi).HasColumnType("datetime");

                entity.Property(e => e.Tpsno)
                    .IsRequired()
                    .HasColumnName("TPSNo")
                    .HasMaxLength(30);

                entity.Property(e => e.UrunKod).HasMaxLength(50);
            });

            modelBuilder.Entity<VwStokGirisDetayListe>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_StokGirisDetayListe");

                entity.Property(e => e.BasvuruTarihi).HasColumnType("datetime");

                entity.Property(e => e.BelgeAd).HasMaxLength(50);

                entity.Property(e => e.BelgeSart).HasMaxLength(50);

                entity.Property(e => e.BeyannameNo).HasMaxLength(16);

                entity.Property(e => e.BeyannameTarihi).HasColumnType("datetime");

                entity.Property(e => e.CikisRejimi).HasMaxLength(4);

                entity.Property(e => e.EsyaCinsi).HasMaxLength(20);

                entity.Property(e => e.EsyaGtip).HasMaxLength(12);

                entity.Property(e => e.FaturaDovizKod).HasMaxLength(3);

                entity.Property(e => e.FaturaNo).HasMaxLength(30);

                entity.Property(e => e.FaturaTarih).HasColumnType("datetime");

                entity.Property(e => e.FaturaTutar).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.GidecegiUlke).HasMaxLength(20);

                entity.Property(e => e.GumrukKod).HasMaxLength(6);

                entity.Property(e => e.Marka).HasMaxLength(50);

                entity.Property(e => e.MenseUlke).HasMaxLength(20);

                entity.Property(e => e.Model).HasMaxLength(50);

                entity.Property(e => e.OlcuBirimi).HasMaxLength(5);

                entity.Property(e => e.Pono)
                    .HasColumnName("PONo")
                    .HasMaxLength(50);

                entity.Property(e => e.Rejim).HasMaxLength(4);

                entity.Property(e => e.SozlesmeUlke).HasMaxLength(20);

                entity.Property(e => e.SureSonuTarihi).HasColumnType("datetime");

                entity.Property(e => e.Tpsaciklama)
                    .HasColumnName("TPSAciklama")
                    .HasMaxLength(250);

                entity.Property(e => e.Tpsbeyan)
                    .HasColumnName("TPSBeyan")
                    .HasMaxLength(20);

                entity.Property(e => e.Tpsdurum)
                    .HasColumnName("TPSDurum")
                    .HasMaxLength(50);

                entity.Property(e => e.Tpsno)
                    .IsRequired()
                    .HasColumnName("TPSNo")
                    .HasMaxLength(30);

                entity.Property(e => e.TpssiraNo).HasColumnName("TPSSiraNo");

                entity.Property(e => e.UrunKod).HasMaxLength(50);
            });

            modelBuilder.Entity<VwSureTakipListe>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_SureTakipListe");

                entity.Property(e => e.BasvuruTarihi).HasColumnType("datetime");

                entity.Property(e => e.BelgeAd).HasMaxLength(50);

                entity.Property(e => e.BelgeSart).HasMaxLength(50);

                entity.Property(e => e.BeyannameNo).HasMaxLength(16);

                entity.Property(e => e.BeyannameTarihi).HasColumnType("datetime");

                entity.Property(e => e.CikisRejimi).HasMaxLength(4);

                entity.Property(e => e.EsyaCinsi).HasMaxLength(20);

                entity.Property(e => e.EsyaGtip).HasMaxLength(12);

                entity.Property(e => e.FaturaDovizKod).HasMaxLength(3);

                entity.Property(e => e.FaturaNo).HasMaxLength(30);

                entity.Property(e => e.FaturaTarih).HasColumnType("datetime");

                entity.Property(e => e.FaturaTutar).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.GidecegiUlke).HasMaxLength(20);

                entity.Property(e => e.GumrukKod).HasMaxLength(6);

                entity.Property(e => e.Marka).HasMaxLength(50);

                entity.Property(e => e.MenseUlke).HasMaxLength(20);

                entity.Property(e => e.Model).HasMaxLength(50);

                entity.Property(e => e.OlcuBirimi).HasMaxLength(5);

                entity.Property(e => e.Pono)
                    .HasColumnName("PONo")
                    .HasMaxLength(50);

                entity.Property(e => e.Rejim).HasMaxLength(4);

                entity.Property(e => e.SozlesmeUlke).HasMaxLength(20);

                entity.Property(e => e.SureSonuTarihi).HasColumnType("datetime");

                entity.Property(e => e.Tpsaciklama)
                    .HasColumnName("TPSAciklama")
                    .HasMaxLength(250);

                entity.Property(e => e.Tpsbeyan)
                    .HasColumnName("TPSBeyan")
                    .HasMaxLength(20);

                entity.Property(e => e.Tpsdurum)
                    .HasColumnName("TPSDurum")
                    .HasMaxLength(50);

                entity.Property(e => e.Tpsno)
                    .IsRequired()
                    .HasColumnName("TPSNo")
                    .HasMaxLength(30);

                entity.Property(e => e.TpssiraNo).HasColumnName("TPSSiraNo");

                entity.Property(e => e.UrunKod).HasMaxLength(50);
            });

            modelBuilder.Entity<VwWsWorkOrderInvoice>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_WsWorkOrderInvoice");

                entity.Property(e => e.AgentName)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.AwbNo)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Blno)
                    .IsRequired()
                    .HasColumnName("BLNo")
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ConsgnAddress).HasMaxLength(500);

                entity.Property(e => e.ConsgnCity).HasMaxLength(100);

                entity.Property(e => e.ConsgnCountry).HasMaxLength(100);

                entity.Property(e => e.ConsgnName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ConsgnNo).HasMaxLength(50);

                entity.Property(e => e.ContainerNo)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.Customs).HasMaxLength(6);

                entity.Property(e => e.DeliveryLocation)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.EntryExitCustoms).HasMaxLength(6);

                entity.Property(e => e.FreightCurrency)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.GtbReferenceNo).HasMaxLength(25);

                entity.Property(e => e.Incoterms).HasMaxLength(3);

                entity.Property(e => e.InsuranceCurrency)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.InvoiceAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InvoiceCurrency).HasMaxLength(3);

                entity.Property(e => e.PaymentMethod).HasMaxLength(2);

                entity.Property(e => e.PlateNo).HasMaxLength(35);

                entity.Property(e => e.SenderAddress).HasMaxLength(500);

                entity.Property(e => e.SenderCity).HasMaxLength(100);

                entity.Property(e => e.SenderCountry).HasMaxLength(100);

                entity.Property(e => e.SenderName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.SenderNo).HasMaxLength(50);

                entity.Property(e => e.TransptrName).HasMaxLength(151);

                entity.Property(e => e.VesselName)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VwWsWorkOrderInvoiceDetails>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_WsWorkOrderInvoiceDetails");

                entity.Property(e => e.CommclDesc)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CountryOfOrigin).HasMaxLength(3);

                entity.Property(e => e.DescGoods)
                    .IsRequired()
                    .HasMaxLength(151);

                entity.Property(e => e.GrossWeight).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.HsCode)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.IncentiveLineNo).HasMaxLength(34);

                entity.Property(e => e.IntrnlAgmt)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.InvoiceAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InvoiceDate).HasColumnType("date");

                entity.Property(e => e.InvoiceNo).HasMaxLength(50);

                entity.Property(e => e.NetWeight).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.PkgType)
                    .IsRequired()
                    .HasMaxLength(2);

                entity.Property(e => e.ProducerCompany)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ProducerCompanyNo)
                    .IsRequired()
                    .HasMaxLength(1)
                    .IsUnicode(false);

                entity.Property(e => e.ProductNo).HasMaxLength(50);

                entity.Property(e => e.Uom)
                    .HasMaxLength(3)
                    .IsFixedLength();
            });

            modelBuilder.Entity<VwWsWorkOrderInvoiceDetailsTcgb>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_WsWorkOrderInvoiceDetailsTcgb");

                entity.Property(e => e.DeclarationDate).HasColumnType("datetime");

                entity.Property(e => e.DeclarationNo).HasMaxLength(16);
            });

            modelBuilder.Entity<VwWsWorkOrderMaster>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_WsWorkOrderMaster");

                entity.Property(e => e.DeclarationType)
                    .IsRequired()
                    .HasMaxLength(2)
                    .IsUnicode(false);

                entity.Property(e => e.PasswordWs).HasMaxLength(20);

                entity.Property(e => e.UserNameWs).HasMaxLength(20);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
