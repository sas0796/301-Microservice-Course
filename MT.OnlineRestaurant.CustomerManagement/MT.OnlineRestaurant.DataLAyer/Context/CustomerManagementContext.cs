using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MT.OnlineRestaurant.DataLayer.Context
{
    public partial class CustomerManagementContext : DbContext
    {
        public CustomerManagementContext()
        {
        }

        public CustomerManagementContext(DbContextOptions<CustomerManagementContext> options)
            : base(options)
        {
        }
        public string DB { get; set; }

        public CustomerManagementContext(string connectionString)
        {
            DB = connectionString;
        }
        public virtual DbSet<LoggingInfo> LoggingInfo { get; set; }
        public virtual DbSet<TblCustomer> TblCustomer { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=tcp:devserver4.database.windows.net;Initial Catalog=CustomerManagement;Persist Security Info=False;User ID=M1043027;Password=Azuredb1@;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("ProductVersion", "2.2.6-servicing-10079");

            modelBuilder.Entity<LoggingInfo>(entity =>
            {
                entity.Property(e => e.ActionName)
                    .HasMaxLength(250)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ControllerName)
                    .HasMaxLength(250)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Description).HasDefaultValueSql("('')");

                entity.Property(e => e.RecordTimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("('')");
            });

            modelBuilder.Entity<TblCustomer>(entity =>
            {
                entity.ToTable("tblCustomer");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address)
                    .IsRequired()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(225)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(225)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(225)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.MobileNumber)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(225)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.PasswordKey)
                   .IsRequired()
                   .HasMaxLength(225)
                   .HasDefaultValueSql("('')");

                entity.Property(e => e.RecordTimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RecordTimeStampCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");
            });
        }
    }
}
