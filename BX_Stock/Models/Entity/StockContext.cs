using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace BX_Stock.Models.Entity
{
    public partial class StockContext : DbContext
    {
        public StockContext()
        {
        }

        public StockContext(DbContextOptions<StockContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Stock> Stock { get; set; }
        public virtual DbSet<StockDay> StockDay { get; set; }
        public virtual DbSet<StockMonth> StockMonth { get; set; }
        public virtual DbSet<StockWeek> StockWeek { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. See http://go.microsoft.com/fwlink/?LinkId=723263 for guidance on storing connection strings.
                optionsBuilder.UseSqlServer("Server=10.1.101.188,1433;Database=Stock; integrated security=true;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Stock>(entity =>
            {
                entity.HasKey(e => e.StockNo);

                entity.Property(e => e.StockNo)
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.IsListed).HasColumnName("isListed");

                entity.Property(e => e.StockName)
                    .IsRequired()
                    .HasMaxLength(10)
                    .IsFixedLength();
            });

            modelBuilder.Entity<StockDay>(entity =>
            {
                entity.HasKey(e => new { e.StockNo, e.Date })
                    .HasName("PK_DayStock");

                entity.Property(e => e.StockNo)
                    .HasColumnName("StockNO")
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Change).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.ClosingPrice).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.D).HasColumnType("decimal(9, 3)");

                entity.Property(e => e.Dea)
                    .HasColumnName("DEA")
                    .HasColumnType("decimal(9, 3)");

                entity.Property(e => e.Dif)
                    .HasColumnName("DIF")
                    .HasColumnType("decimal(9, 3)");

                entity.Property(e => e.Ema12)
                    .HasColumnName("EMA12")
                    .HasColumnType("decimal(9, 3)");

                entity.Property(e => e.Ema26)
                    .HasColumnName("EMA26")
                    .HasColumnType("decimal(9, 3)");

                entity.Property(e => e.HighestPrice).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.K).HasColumnType("decimal(9, 3)");

                entity.Property(e => e.LowestPrice).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.OpeningPrice).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.Osc)
                    .HasColumnName("OSC")
                    .HasColumnType("decimal(9, 3)");
            });

            modelBuilder.Entity<StockMonth>(entity =>
            {
                entity.HasKey(e => new { e.StockNo, e.Date });

                entity.Property(e => e.StockNo)
                    .HasColumnName("StockNO")
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Change).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.ClosingPrice).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.D).HasColumnType("decimal(9, 3)");

                entity.Property(e => e.Dea)
                    .HasColumnName("DEA")
                    .HasColumnType("decimal(9, 3)");

                entity.Property(e => e.Dif)
                    .HasColumnName("DIF")
                    .HasColumnType("decimal(9, 3)");

                entity.Property(e => e.Ema12)
                    .HasColumnName("EMA12")
                    .HasColumnType("decimal(9, 3)");

                entity.Property(e => e.Ema26)
                    .HasColumnName("EMA26")
                    .HasColumnType("decimal(9, 3)");

                entity.Property(e => e.HighestPrice).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.K).HasColumnType("decimal(9, 3)");

                entity.Property(e => e.LowestPrice).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.OpeningPrice).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.Osc)
                    .HasColumnName("OSC")
                    .HasColumnType("decimal(9, 3)");
            });

            modelBuilder.Entity<StockWeek>(entity =>
            {
                entity.HasKey(e => new { e.StockNo, e.Date });

                entity.Property(e => e.StockNo)
                    .HasColumnName("StockNO")
                    .HasMaxLength(4)
                    .IsUnicode(false)
                    .IsFixedLength();

                entity.Property(e => e.Date).HasColumnType("datetime");

                entity.Property(e => e.Change).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.ClosingPrice).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.D).HasColumnType("decimal(9, 3)");

                entity.Property(e => e.Dea)
                    .HasColumnName("DEA")
                    .HasColumnType("decimal(9, 3)");

                entity.Property(e => e.Dif)
                    .HasColumnName("DIF")
                    .HasColumnType("decimal(9, 3)");

                entity.Property(e => e.Ema12)
                    .HasColumnName("EMA12")
                    .HasColumnType("decimal(9, 3)");

                entity.Property(e => e.Ema26)
                    .HasColumnName("EMA26")
                    .HasColumnType("decimal(9, 3)");

                entity.Property(e => e.HighestPrice).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.K).HasColumnType("decimal(9, 3)");

                entity.Property(e => e.LowestPrice).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.OpeningPrice).HasColumnType("decimal(9, 2)");

                entity.Property(e => e.Osc)
                    .HasColumnName("OSC")
                    .HasColumnType("decimal(9, 3)");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
