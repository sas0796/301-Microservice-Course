using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel
{
    public partial class RestaurantManagementContext : DbContext
    {

        //private readonly string DbConnectionString;
        public RestaurantManagementContext()
        {
        }

        public RestaurantManagementContext(DbContextOptions<RestaurantManagementContext> options)
            : base(options)
        {
        }
        //public RestaurantManagementContext(string connectionstring)
        //{
        //    DbConnectionString = connectionstring;
        //}
        public virtual DbSet<LoggingInfo> LoggingInfo { get; set; }
        public virtual DbSet<TblCuisine> TblCuisine { get; set; }
        public virtual DbSet<TblLocation> TblLocation { get; set; }
        public virtual DbSet<TblMenu> TblMenu { get; set; }
        public virtual DbSet<TblOffer> TblOffer { get; set; }
        public virtual DbSet<TblRating> TblRating { get; set; }
        public virtual DbSet<TblRestaurant> TblRestaurant { get; set; }
        public virtual DbSet<TblRestaurantDetails> TblRestaurantDetails { get; set; }
        public virtual DbSet<TblRestaurantMenuPrice> TblRestaurantMenuPrice { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                // optionsBuilder.UseSqlServer(@"Server=tcp:capstoneteam1server.database.windows.net,1433;Initial Catalog=RestaurantManagement;Persist Security Info=False;user id=cpadmin;password=Mindtree@12;");
                //optionsBuilder.UseSqlServer(DbConnectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
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

            modelBuilder.Entity<TblRestaurantMenuPrice>(entity =>
            {
                entity.ToTable("tblRestaurantMenuPrice");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.tblRestaurantId)
                    .HasColumnName("tblRestaurantID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.tblMenuId)
                    .HasColumnName("tblMenuID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Price).HasColumnType("decimal(10,2)")
                    .HasColumnName("Price")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Quantity)
                    .HasColumnName("Quantity")
                    .HasDefaultValueSql("((0))");

                entity.HasOne(d => d.TblRestaurant)
                    .WithMany(p => p.TblRestaurantMenuPrice)
                    .HasForeignKey(d => d.tblRestaurantId)
                    .HasConstraintName("FK_tblRestaurant_tblRestaurantId");

                entity.HasOne(d => d.TblMenu)
                    .WithMany(p => p.TblRestaurantMenuPrice)
                    .HasForeignKey(d => d.tblMenuId)
                    .HasConstraintName("FK_tblMenu_tblMenuId");
            });

            modelBuilder.Entity<TblCuisine>(entity =>
            {
                entity.ToTable("tblCuisine");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Cuisine)
                    .IsRequired()
                    .HasMaxLength(225)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RecordTimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RecordTimeStampCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UserCreated).HasDefaultValueSql("((0))");

                entity.Property(e => e.UserModified).HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblLocation>(entity =>
            {
                entity.ToTable("tblLocation");

                entity.HasIndex(e => e.X)
                    .HasName("UQ__tblLocat__3BD0198414754610")
                    .IsUnique();

                entity.HasIndex(e => e.Y)
                    .HasName("UQ__tblLocat__3BD01987EC697B94")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RecordTimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RecordTimeStampCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UserCreated).HasDefaultValueSql("((0))");

                entity.Property(e => e.UserModified).HasDefaultValueSql("((0))");

                entity.Property(e => e.X).HasColumnType("decimal(18, 0)");

                entity.Property(e => e.Y).HasColumnType("decimal(18, 0)");
            });

            modelBuilder.Entity<TblMenu>(entity =>
            {
                entity.ToTable("tblMenu");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Item)
                    .IsRequired()
                    .HasMaxLength(225)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RecordTimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RecordTimeStampCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TblCuisineId)
                    .HasColumnName("tblCuisineID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UserCreated).HasDefaultValueSql("((0))");

                entity.Property(e => e.UserModified).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.TblCuisine)
                    .WithMany(p => p.TblMenu)
                    .HasForeignKey(d => d.TblCuisineId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblMenu_tblCuisineID");

                entity.Property(e => e.quantity)
                    .HasColumnName("Quantity")
                    .HasDefaultValueSql("((0))");
            });

            modelBuilder.Entity<TblOffer>(entity =>
            {
                entity.ToTable("tblOffer");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Discount).HasDefaultValueSql("((0))");

                entity.Property(e => e.FromDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Price).HasColumnType("decimal").HasDefaultValueSql("((0))");

                entity.Property(e => e.RecordTimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RecordTimeStampCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TblMenuId)
                    .HasColumnName("tblMenuID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TblRestaurantId)
                    .HasColumnName("tblRestaurantID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.ToDate)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UserCreated).HasDefaultValueSql("((0))");

                entity.Property(e => e.UserModified).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.TblMenu)
                    .WithMany(p => p.TblOffer)
                    .HasForeignKey(d => d.TblMenuId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblOffer_tblMenuID");

                entity.HasOne(d => d.TblRestaurant)
                    .WithMany(p => p.TblOffer)
                    .HasForeignKey(d => d.TblRestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblOffer_tblRestaurantID");
            });

            modelBuilder.Entity<TblRating>(entity =>
            {
                entity.ToTable("tblRating");

                entity.Property(e => e.Id).HasColumnName("ID");


                entity.Property(e => e.Comments)
                    .IsRequired()
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Rating)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RecordTimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RecordTimeStampCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TblCustomerId)
                    .HasColumnName("tblCustomerId")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TblRestaurantId)
                    .HasColumnName("tblRestaurantID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UserCreated).HasDefaultValueSql("((0))");

                entity.Property(e => e.UserModified).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.TblRestaurant)
                    .WithMany(p => p.TblRating)
                    .HasForeignKey(d => d.TblRestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRating_tblRestaurantID");
            });

            modelBuilder.Entity<TblRestaurant>(entity =>
            {
                entity.ToTable("tblRestaurant");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.Address).IsUnicode(false);

                entity.Property(e => e.CloseTime)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.ContactNo)
                    .IsRequired()
                    .HasMaxLength(20)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(225)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.OpeningTime)
                    .IsRequired()
                    .HasMaxLength(5)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RecordTimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RecordTimeStampCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TblLocationId)
                    .HasColumnName("tblLocationID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UserCreated).HasDefaultValueSql("((0))");

                entity.Property(e => e.UserModified).HasDefaultValueSql("((0))");

                entity.Property(e => e.Website)
                    .IsRequired()
                    .HasMaxLength(225)
                    .HasDefaultValueSql("('')");

                entity.HasOne(d => d.TblLocation)
                    .WithMany(p => p.TblRestaurant)
                    .HasForeignKey(d => d.TblLocationId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRestaurant_tblLocationID");
            });

            modelBuilder.Entity<TblRestaurantDetails>(entity =>
            {
                entity.ToTable("tblRestaurantDetails");

                entity.Property(e => e.Id).HasColumnName("ID");

                entity.Property(e => e.RecordTimeStamp)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RecordTimeStampCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TableCapacity).HasDefaultValueSql("((0))");

                entity.Property(e => e.TableCount).HasDefaultValueSql("((0))");

                entity.Property(e => e.TblRestaurantId)
                    .HasColumnName("tblRestaurantID")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.UserCreated).HasDefaultValueSql("((0))");

                entity.Property(e => e.UserModified).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.TblRestaurant)
                    .WithMany(p => p.TblRestaurantDetails)
                    .HasForeignKey(d => d.TblRestaurantId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_tblRestaurantDetails_tblRestaurantID");
            });
        }
    }
}
