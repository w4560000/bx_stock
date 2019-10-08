using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BX_Stock.Models.Entity
{
    public partial class BxstockContext : DbContext
    {
        public BxstockContext()
        {
        }

        public BxstockContext(DbContextOptions<BxstockContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Test> Test { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
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

            this.OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
