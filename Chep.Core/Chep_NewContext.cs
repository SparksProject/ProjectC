using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

// Code scaffolded by EF Core assumes nullable reference types (NRTs) are not used or disabled.
// If you have enabled NRTs for your project, then un-comment the following line:
// #nullable disable

namespace Chep.Core
{
    public partial class Chep_NewContext : DbContext
    {
        public Chep_NewContext()
        {
        }

        public Chep_NewContext(DbContextOptions<Chep_NewContext> options)
            : base(options)
        {
        }

        public virtual DbSet<ChepStokCikis> ChepStokCikis { get; set; }
        public virtual DbSet<ChepStokCikisDetay> ChepStokCikisDetay { get; set; }
        public virtual DbSet<ChepStokGiris> ChepStokGiris { get; set; }
        public virtual DbSet<ChepStokGirisDetay> ChepStokGirisDetay { get; set; }
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Country> Country { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<Customs> Customs { get; set; }
        public virtual DbSet<ExceptionLog> ExceptionLog { get; set; }
        public virtual DbSet<GenericReport> GenericReport { get; set; }
        public virtual DbSet<GenericReportParameter> GenericReportParameter { get; set; }
        public virtual DbSet<GenericReportUser> GenericReportUser { get; set; }
        public virtual DbSet<MailDefinition> MailDefinition { get; set; }
        public virtual DbSet<MailReport> MailReport { get; set; }
        public virtual DbSet<MailReportUser> MailReportUser { get; set; }
        public virtual DbSet<PeriodType> PeriodType { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<RecordStatus> RecordStatus { get; set; }
        public virtual DbSet<SentMail> SentMail { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserCustomer> UserCustomer { get; set; }
        public virtual DbSet<UserPermission> UserPermission { get; set; }
        public virtual DbSet<UserType> UserType { get; set; }
        public virtual DbSet<VwGenelListe> VwGenelListe { get; set; }
        public virtual DbSet<VwStokCikisDetayListe> VwStokCikisDetayListe { get; set; }
        public virtual DbSet<VwStokDurumListe> VwStokDurumListe { get; set; }
        public virtual DbSet<VwStokDusumListe> VwStokDusumListe { get; set; }
        public virtual DbSet<VwStokGirisDetayListe> VwStokGirisDetayListe { get; set; }
        public virtual DbSet<VwSureTakipListe> VwSureTakipListe { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=.;Database=Chep_New;Trusted_Connection=False;User Id=necmi;Password=@Necmi*");
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

                entity.Property(e => e.IslemTarihi).HasColumnType("datetime");

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

                entity.Property(e => e.Kg).HasColumnType("decimal(8, 2)");

                entity.HasOne(d => d.StokCikis)
                    .WithMany(p => p.ChepStokCikisDetay)
                    .HasForeignKey(d => d.StokCikisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
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

                entity.Property(e => e.BasvuruTarihi).HasColumnType("datetime");

                entity.Property(e => e.BelgeAd).HasMaxLength(50);

                entity.Property(e => e.BelgeSart).HasMaxLength(50);

                entity.Property(e => e.BeyannameNo).HasMaxLength(16);

                entity.Property(e => e.BeyannameTarihi).HasColumnType("datetime");

                entity.Property(e => e.GumrukKod).HasMaxLength(6);

                entity.Property(e => e.SureSonuTarihi).HasColumnType("datetime");

                entity.Property(e => e.TpsAciklama).HasMaxLength(250);

                entity.Property(e => e.TpsDurum)
                    .IsRequired()
                    .HasMaxLength(50);

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

                entity.Property(e => e.CikisRejimi).HasMaxLength(4);

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

                entity.Property(e => e.Rejim).HasMaxLength(4);

                entity.Property(e => e.SozlesmeUlke).HasMaxLength(20);

                entity.Property(e => e.TpsBeyan).HasMaxLength(20);

                entity.Property(e => e.UrunKod).HasMaxLength(50);

                entity.HasOne(d => d.StokGiris)
                    .WithMany(p => p.ChepStokGirisDetay)
                    .HasForeignKey(d => d.StokGirisId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
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

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.Adress)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.Country)
                    .IsRequired()
                    .HasMaxLength(100);

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

                entity.Property(e => e.TaxName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.TaxNo)
                    .IsRequired()
                    .HasMaxLength(20);

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

            modelBuilder.Entity<PeriodType>(entity =>
            {
                entity.Property(e => e.PeriodTypeName)
                    .IsRequired()
                    .HasMaxLength(150);
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.Property(e => e.ProductId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.HsCode)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.ProductNameEng).HasMaxLength(100);

                entity.Property(e => e.ProductNameOrg).HasMaxLength(100);

                entity.Property(e => e.ProductNameTr)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.ProductNo)
                    .IsRequired()
                    .HasMaxLength(50);

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

                entity.Property(e => e.CikisIhracatciFirma).HasMaxLength(50);

                entity.Property(e => e.CikisIslemTarihi).HasColumnType("datetime");

                entity.Property(e => e.CikisReferansNo).HasMaxLength(20);

                entity.Property(e => e.CikisRejimi).HasMaxLength(4);

                entity.Property(e => e.CikisTpsno)
                    .HasColumnName("CikisTPSNo")
                    .HasMaxLength(50);

                entity.Property(e => e.CikisTpstarih)
                    .HasColumnName("CikisTPSTarih")
                    .HasColumnType("datetime");

                entity.Property(e => e.EsyaCinsi).HasMaxLength(20);

                entity.Property(e => e.EsyaGtip)
                    .HasColumnName("EsyaGTIP")
                    .HasMaxLength(12);

                entity.Property(e => e.FaturaDovizKod).HasMaxLength(3);

                entity.Property(e => e.FaturaTutar).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.GidecegiUlke).HasMaxLength(20);

                entity.Property(e => e.GirisBeyannameNo).HasMaxLength(16);

                entity.Property(e => e.GirisBeyannameTarihi).HasColumnType("datetime");

                entity.Property(e => e.GirisFaturaNo).HasMaxLength(30);

                entity.Property(e => e.GirisFaturaTarih).HasColumnType("datetime");

                entity.Property(e => e.GirisReferansNo)
                    .IsRequired()
                    .HasMaxLength(16);

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
                    .IsRequired()
                    .HasColumnName("TPSDurum")
                    .HasMaxLength(50);

                entity.Property(e => e.UrunKod).HasMaxLength(50);
            });

            modelBuilder.Entity<VwStokCikisDetayListe>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vw_StokCikisDetayListe");

                entity.Property(e => e.BeyannameNo).HasMaxLength(20);

                entity.Property(e => e.BeyannameTarihi).HasColumnType("datetime");

                entity.Property(e => e.EsyaCinsi).HasMaxLength(20);

                entity.Property(e => e.EsyaGtip)
                    .HasColumnName("EsyaGTIP")
                    .HasMaxLength(12);

                entity.Property(e => e.IhracatciFirma).HasMaxLength(50);

                entity.Property(e => e.IslemTarihi).HasColumnType("datetime");

                entity.Property(e => e.Marka).HasMaxLength(50);

                entity.Property(e => e.Model).HasMaxLength(50);

                entity.Property(e => e.Pono)
                    .HasColumnName("PONo")
                    .HasMaxLength(50);

                entity.Property(e => e.ReferansNo).HasMaxLength(20);

                entity.Property(e => e.Tpsno)
                    .HasColumnName("TPSNo")
                    .HasMaxLength(50);

                entity.Property(e => e.Tpstarih)
                    .HasColumnName("TPSTarih")
                    .HasColumnType("datetime");

                entity.Property(e => e.UrunKod).HasMaxLength(50);
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

                entity.Property(e => e.CikisRejimi).HasMaxLength(4);

                entity.Property(e => e.EsyaCinsi).HasMaxLength(20);

                entity.Property(e => e.EsyaGtip)
                    .HasColumnName("EsyaGTIP")
                    .HasMaxLength(12);

                entity.Property(e => e.FaturaDovizKod).HasMaxLength(3);

                entity.Property(e => e.FaturaNo).HasMaxLength(30);

                entity.Property(e => e.FaturaTarih).HasColumnType("datetime");

                entity.Property(e => e.FaturaTutar).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.GidecegiUlke).HasMaxLength(20);

                entity.Property(e => e.GumrukKod).HasMaxLength(6);

                entity.Property(e => e.Marka).HasMaxLength(50);

                entity.Property(e => e.MenseUlke).HasMaxLength(20);

                entity.Property(e => e.Model).HasMaxLength(50);

                entity.Property(e => e.OlcuBirimi).HasMaxLength(5);

                entity.Property(e => e.Pono)
                    .HasColumnName("PONo")
                    .HasMaxLength(50);

                entity.Property(e => e.ReferansNo)
                    .IsRequired()
                    .HasMaxLength(16);

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
                    .IsRequired()
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

                entity.Property(e => e.GirisBeyannameNo).HasMaxLength(16);

                entity.Property(e => e.GirisBeyannameTarihi).HasColumnType("datetime");

                entity.Property(e => e.GirisReferansNo)
                    .IsRequired()
                    .HasMaxLength(16);

                entity.Property(e => e.Marka).HasMaxLength(50);

                entity.Property(e => e.Model).HasMaxLength(50);

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

                entity.Property(e => e.EsyaGtip)
                    .HasColumnName("EsyaGTIP")
                    .HasMaxLength(12);

                entity.Property(e => e.FaturaDovizKod).HasMaxLength(3);

                entity.Property(e => e.FaturaNo).HasMaxLength(30);

                entity.Property(e => e.FaturaTarih).HasColumnType("datetime");

                entity.Property(e => e.FaturaTutar).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.GidecegiUlke).HasMaxLength(20);

                entity.Property(e => e.GumrukKod).HasMaxLength(6);

                entity.Property(e => e.Marka).HasMaxLength(50);

                entity.Property(e => e.MenseUlke).HasMaxLength(20);

                entity.Property(e => e.Model).HasMaxLength(50);

                entity.Property(e => e.OlcuBirimi).HasMaxLength(5);

                entity.Property(e => e.Pono)
                    .HasColumnName("PONo")
                    .HasMaxLength(50);

                entity.Property(e => e.ReferansNo)
                    .IsRequired()
                    .HasMaxLength(16);

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
                    .IsRequired()
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

                entity.Property(e => e.EsyaGtip)
                    .HasColumnName("EsyaGTIP")
                    .HasMaxLength(12);

                entity.Property(e => e.FaturaDovizKod).HasMaxLength(3);

                entity.Property(e => e.FaturaNo).HasMaxLength(30);

                entity.Property(e => e.FaturaTarih).HasColumnType("datetime");

                entity.Property(e => e.FaturaTutar).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.GidecegiUlke).HasMaxLength(20);

                entity.Property(e => e.GumrukKod).HasMaxLength(6);

                entity.Property(e => e.Marka).HasMaxLength(50);

                entity.Property(e => e.MenseUlke).HasMaxLength(20);

                entity.Property(e => e.Model).HasMaxLength(50);

                entity.Property(e => e.OlcuBirimi).HasMaxLength(5);

                entity.Property(e => e.Pono)
                    .HasColumnName("PONo")
                    .HasMaxLength(50);

                entity.Property(e => e.ReferansNo)
                    .IsRequired()
                    .HasMaxLength(16);

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
                    .IsRequired()
                    .HasColumnName("TPSDurum")
                    .HasMaxLength(50);

                entity.Property(e => e.Tpsno)
                    .IsRequired()
                    .HasColumnName("TPSNo")
                    .HasMaxLength(30);

                entity.Property(e => e.TpssiraNo).HasColumnName("TPSSiraNo");

                entity.Property(e => e.UrunKod).HasMaxLength(50);
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
