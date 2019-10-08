using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BX_Stock.Models.Entity
{
    public partial class bxstockContext : DbContext
    {
        public bxstockContext()
        {
        }

        public bxstockContext(DbContextOptions<bxstockContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Test> Test { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=bxsqlserver.database.windows.net;Database=bxstock;User Id=bingxiang;Password=Aa334567;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Test>(entity =>
            {
                entity.HasNoKey();

                entity.Property(e => e.TestId)
                    .HasColumnName("Test_id")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.TestString)
                    .HasColumnName("Test_string")
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
