using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace MT.OnlineRestaurant.DataLayer.EntityFrameWorkModel
{
    public partial class ReviewManagementContext : DbContext
    {

        //private readonly string DbConnectionString;
        public ReviewManagementContext()
        {
        }

        public ReviewManagementContext(DbContextOptions<ReviewManagementContext> options)
            : base(options)
        {
        }
        public virtual DbSet<TblRating> TblRating { get; set; }
        public virtual DbSet<TblRestaurant> TblRestaurant { get; set; }
        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TblRating>(entity =>
            {
                entity.ToTable("tblRating");

                entity.Property(e => e.Id).HasColumnName("ID");


                entity.Property(e => e.Comments)
                    .HasMaxLength(250)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.Rating)
                    .IsRequired()
                    .HasMaxLength(10)
                    .HasDefaultValueSql("('')");

                entity.Property(e => e.RecordTimeStampCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RecordTimeStampUpdated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.TblRestaurantId)
                    .HasColumnName("tblRestaurantID")
                    .HasDefaultValueSql("((0))");

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

                entity.Property(e => e.RecordTimeStampCreated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.RecordTimeStampUpdated)
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.Website)
                    .IsRequired()
                    .HasMaxLength(225)
                    .HasDefaultValueSql("('')");
            });
        }
    }
}
