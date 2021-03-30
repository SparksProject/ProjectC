using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace ProjectC.Data.Models
{
    public partial class ProjectCContext : DbContext
    {
        public ProjectCContext()
        {
        }

        public ProjectCContext(DbContextOptions<ProjectCContext> options)
            : base(options)
        {
        }

        public ProjectCContext(DbContextOptions options)
    : base(options)
        {
        }

       
        public virtual DbSet<Company> Company { get; set; }
        public virtual DbSet<Customer> Customer { get; set; }
        public virtual DbSet<ExceptionLog> ExceptionLog { get; set; }
        public virtual DbSet<GenericReport> GenericReport { get; set; }
        public virtual DbSet<GenericReportParameter> GenericReportParameter { get; set; }
        public virtual DbSet<GenericReportUser> GenericReportUser { get; set; }
        public virtual DbSet<Invoice> Invoice { get; set; }
        public virtual DbSet<InvoiceDetail> InvoiceDetail { get; set; }
        public virtual DbSet<MailDefinition> MailDefinition { get; set; }
        public virtual DbSet<MailReport> MailReport { get; set; }
        public virtual DbSet<MailReportUser> MailReportUser { get; set; }
        public virtual DbSet<PeriodType> PeriodType { get; set; }
        public virtual DbSet<Product> Product { get; set; }
        public virtual DbSet<RecordStatus> RecordStatus { get; set; }
        public virtual DbSet<SentMail> SentMail { get; set; }
        public virtual DbSet<User> User { get; set; }
        public virtual DbSet<UserPermission> UserPermission { get; set; }
        public virtual DbSet<UserType> UserType { get; set; }
        public virtual DbSet<WorkOrderMaster> WorkOrderMaster { get; set; }
        public virtual DbSet<Teminat> Teminat { get; set; }
        public virtual DbSet<SparksArchive> SparksArchive { get; set; }
        public virtual DbSet<Arsiv> Arsiv { get; set; }
        public virtual DbSet<ViewSparksArchive> ViewSparksArchive { get; set; }
        public virtual DbSet<Beyanname> Beyanname { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();

                var connectionString = configuration.GetSection("SparksXConfig")["ConnectionString"];

                optionsBuilder.UseSqlServer(connectionString);
            }

            optionsBuilder.UseLazyLoadingProxies(true);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<Company>(entity =>
            {
                entity.Property(e => e.CompanyId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CompanyName)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.SmtpHost).HasMaxLength(100);

                entity.Property(e => e.SmtpIsSSLEnabled).HasColumnName("SmtpIsSSLEnabled");

                entity.Property(e => e.SmtpPassword).HasMaxLength(50);

                entity.Property(e => e.SmtpUserName).HasMaxLength(100);

                entity.Property(e => e.ArchivePath).HasMaxLength(250);

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

            modelBuilder.Entity<Customer>(entity =>
            {
                entity.Property(e => e.CustomerId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(100);

                entity.Property(e => e.OtherId).HasMaxLength(20);

                entity.Property(e => e.PasswordWs).HasMaxLength(20);

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

                entity.Property(e => e.ParameterLabel).HasMaxLength(100);

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
                    .HasConstraintName("FK__GenericRe__Gener__17F790F9");

                entity.HasOne(d => d.User)
                    .WithMany(p => p.GenericReportUser)
                    .HasForeignKey(d => d.UserId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK__GenericRe__UserI__18EBB532");
            });

            modelBuilder.Entity<Invoice>(entity =>
            {
                entity.Property(e => e.InvoiceId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.AgentName).HasMaxLength(250);

                entity.Property(e => e.AwbNo).HasMaxLength(30);

                entity.Property(e => e.Blno)
                    .HasColumnName("BLNo")
                    .HasMaxLength(25);

                entity.Property(e => e.ConsgnAddress).HasMaxLength(2000);

                entity.Property(e => e.ConsgnCity).HasMaxLength(60);

                entity.Property(e => e.ConsgnCountry).HasMaxLength(25);

                entity.Property(e => e.ConsgnName).HasMaxLength(250);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeliveryLocation).HasMaxLength(250);

                entity.Property(e => e.FreightAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.FreightCurrency).HasMaxLength(6);

                entity.Property(e => e.Incoterms).HasMaxLength(25);

                entity.Property(e => e.InsuranceAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InsuranceCurrency).HasMaxLength(6);

                entity.Property(e => e.InvoiceAmount).HasColumnType("decimal(18, 2)");

                entity.Property(e => e.InvoiceCurrency)
                    .IsRequired()
                    .HasMaxLength(6);

                entity.Property(e => e.PlateNo).HasMaxLength(50);

                entity.Property(e => e.SenderAddress).HasMaxLength(2000);

                entity.Property(e => e.SenderCity).HasMaxLength(60);

                entity.Property(e => e.SenderCountry).HasMaxLength(25);

                entity.Property(e => e.SenderName).HasMaxLength(250);

                entity.Property(e => e.SenderNo).HasMaxLength(30);

                entity.Property(e => e.TransptrName).HasMaxLength(25);

                entity.Property(e => e.VesselName).HasMaxLength(250);

                entity.HasOne(d => d.WorkOrderMaster)
                    .WithMany(p => p.Invoice)
                    .HasForeignKey(d => d.WorkOrderMasterId)
                    .HasConstraintName("FK_Invoices_WorkOrderMasters");
            });

            modelBuilder.Entity<InvoiceDetail>(entity =>
            {
                entity.Property(e => e.InvoiceDetailId).HasDefaultValueSql("(newid())");

                entity.Property(e => e.CommclDesc).HasMaxLength(240);

                entity.Property(e => e.CountryOfOrigin).HasMaxLength(50);

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.DescGoods)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.HsCode).HasMaxLength(16);

                entity.Property(e => e.IntrnlAgmt).HasMaxLength(10);

                entity.Property(e => e.InvoiceDate).HasColumnType("date");

                entity.Property(e => e.InvoiceNo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.PkgType)
                    .IsRequired()
                    .HasMaxLength(10);

                entity.Property(e => e.ProductNo)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.Uom)
                    .IsRequired()
                    .HasMaxLength(3);

                entity.HasOne(d => d.Invoice)
                    .WithMany(p => p.InvoiceDetail)
                    .HasForeignKey(d => d.InvoiceId)
                    .HasConstraintName("FK_InvoiceDetails_Invoices");
            });

            modelBuilder.Entity<MailDefinition>(entity =>
            {
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

                entity.Property(e => e.PeriodValue).HasMaxLength(100);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.MailReportName)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

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

                entity.Property(e => e.Uom).HasMaxLength(3);

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

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(150);

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
                    .HasMaxLength(20);

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

            modelBuilder.Entity<UserPermission>(entity =>
            {
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

            modelBuilder.Entity<WorkOrderMaster>(entity =>
            {
                entity.Property(e => e.WorkOrderMasterId).ValueGeneratedNever();

                entity.Property(e => e.CreatedDate).HasColumnType("datetime");

                entity.Property(e => e.DeclarationType)
                    .IsRequired()
                    .HasMaxLength(2);

                entity.Property(e => e.DeletedDate).HasColumnType("datetime");

                entity.Property(e => e.Message).HasColumnType("ntext");

                entity.Property(e => e.ModifiedDate).HasColumnType("datetime");

                entity.Property(e => e.WorkOrderNo)
                    .IsRequired()
                    .HasMaxLength(30);

                entity.HasOne(d => d.CreatedByNavigation)
                    .WithMany(p => p.WorkOrderMasterCreatedByNavigation)
                    .HasForeignKey(d => d.CreatedBy)
                    .HasConstraintName("FK_WorkOrderMaster_Users");

                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.WorkOrderMaster)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkOrderMaster_Customer");

                entity.HasOne(d => d.DeletedByNavigation)
                    .WithMany(p => p.WorkOrderMasterDeletedByNavigation)
                    .HasForeignKey(d => d.DeletedBy)
                    .HasConstraintName("FK_WorkOrderMaster_Users2");

                entity.HasOne(d => d.ModifiedByNavigation)
                    .WithMany(p => p.WorkOrderMasterModifiedByNavigation)
                    .HasForeignKey(d => d.ModifiedBy)
                    .HasConstraintName("FK_WorkOrderMaster_Users1");

                entity.HasOne(d => d.RecordStatus)
                    .WithMany(p => p.WorkOrderMaster)
                    .HasForeignKey(d => d.RecordStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_WorkOrderMaster_RecordStatuses");
            });

            modelBuilder.Entity<Teminat>(entity => {
                entity.Property(e => e.DurumId).IsRequired();
                entity.Property(e => e.TeminatTipiId).IsRequired();
                entity.Property(e => e.DosyaTipi).HasMaxLength(250);
                entity.Property(e => e.DosyaNo).HasMaxLength(50);
                entity.Property(e => e.Gonderici).HasMaxLength(250);
                entity.Property(e => e.Alici).HasMaxLength(250);
            });
            modelBuilder.Entity<SparksArchive>(entity =>
            {
                entity.Property(e => e.ArchiveId).ValueGeneratedNever();
                entity.HasOne(d => d.Customer)
                    .WithMany(p => p.SparksArchive)
                    .HasForeignKey(d => d.CustomerId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_SparksArchive_Customer");

                entity.Property(e => e.DosyaTipi).HasMaxLength(250);
                entity.Property(e => e.DosyaNo).HasMaxLength(250);
                entity.Property(e => e.BelgeAdi).HasMaxLength(250);
               // entity.Property(e => e.DosyaYolu).HasMaxLength(500);
               // entity.Property(e => e.FileDate).HasMaxLength(50);

            });
        }
    }
}