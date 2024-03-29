﻿// <auto-generated />
using System;
using BX_Stock.Models.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace BX_Stock.Migrations
{
    [DbContext(typeof(StockContext))]
    [Migration("20210612161054_init")]
    partial class init
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("BX_Stock.Models.Entity.Stock", b =>
                {
                    b.Property<int>("StockNo")
                        .HasColumnType("int")
                        .IsFixedLength(true)
                        .HasMaxLength(4)
                        .IsUnicode(false);

                    b.Property<bool>("IsListed")
                        .HasColumnType("bit");

                    b.Property<string>("StockName")
                        .IsRequired()
                        .HasColumnType("nchar(10)")
                        .IsFixedLength(true)
                        .HasMaxLength(10)
                        .IsUnicode(true);

                    b.HasKey("StockNo");

                    b.ToTable("Stock");
                });

            modelBuilder.Entity("BX_Stock.Models.Entity.StockDay", b =>
                {
                    b.Property<int>("StockNo")
                        .HasColumnType("int")
                        .IsFixedLength(true)
                        .HasMaxLength(4)
                        .IsUnicode(true);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<decimal>("Change")
                        .HasColumnType("decimal(9, 2)");

                    b.Property<decimal>("ClosingPrice")
                        .HasColumnType("decimal(9, 2)");

                    b.Property<decimal>("D")
                        .HasColumnType("decimal(9, 3)");

                    b.Property<decimal>("Dea")
                        .HasColumnType("decimal(9, 3)");

                    b.Property<decimal>("Dif")
                        .HasColumnType("decimal(9, 3)");

                    b.Property<decimal>("Ema12")
                        .HasColumnType("decimal(9, 3)");

                    b.Property<decimal>("Ema26")
                        .HasColumnType("decimal(9, 3)");

                    b.Property<decimal>("HighestPrice")
                        .HasColumnType("decimal(9, 2)");

                    b.Property<decimal>("K")
                        .HasColumnType("decimal(9, 3)");

                    b.Property<decimal>("LowestPrice")
                        .HasColumnType("decimal(9, 2)");

                    b.Property<decimal>("OpeningPrice")
                        .HasColumnType("decimal(9, 2)");

                    b.Property<decimal>("Osc")
                        .HasColumnType("decimal(9, 3)");

                    b.Property<long>("TradeValue")
                        .HasColumnType("bigint");

                    b.Property<int>("Transaction")
                        .HasColumnType("int");

                    b.HasKey("StockNo", "Date")
                        .HasName("PK_DayStock");

                    b.ToTable("StockDay");
                });

            modelBuilder.Entity("BX_Stock.Models.Entity.StockMonth", b =>
                {
                    b.Property<int>("StockNo")
                        .HasColumnType("int")
                        .IsFixedLength(true)
                        .HasMaxLength(4)
                        .IsUnicode(false);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<decimal>("Change")
                        .HasColumnType("decimal(9, 2)");

                    b.Property<decimal>("ClosingPrice")
                        .HasColumnType("decimal(9, 2)");

                    b.Property<decimal>("D")
                        .HasColumnType("decimal(9, 3)");

                    b.Property<decimal>("Dea")
                        .HasColumnType("decimal(9, 3)");

                    b.Property<decimal>("Dif")
                        .HasColumnType("decimal(9, 3)");

                    b.Property<decimal>("Ema12")
                        .HasColumnType("decimal(9, 3)");

                    b.Property<decimal>("Ema26")
                        .HasColumnType("decimal(9, 3)");

                    b.Property<decimal>("HighestPrice")
                        .HasColumnType("decimal(9, 2)");

                    b.Property<decimal>("K")
                        .HasColumnType("decimal(9, 3)");

                    b.Property<decimal>("LowestPrice")
                        .HasColumnType("decimal(9, 2)");

                    b.Property<decimal>("OpeningPrice")
                        .HasColumnType("decimal(9, 2)");

                    b.Property<decimal>("Osc")
                        .HasColumnType("decimal(9, 3)");

                    b.Property<long>("TradeValue")
                        .HasColumnType("bigint");

                    b.Property<int>("Transaction")
                        .HasColumnType("int");

                    b.HasKey("StockNo", "Date");

                    b.ToTable("StockMonth");
                });

            modelBuilder.Entity("BX_Stock.Models.Entity.StockWeek", b =>
                {
                    b.Property<int>("StockNo")
                        .HasColumnType("int")
                        .IsFixedLength(true)
                        .HasMaxLength(4)
                        .IsUnicode(false);

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime");

                    b.Property<decimal>("Change")
                        .HasColumnType("decimal(9, 2)");

                    b.Property<decimal>("ClosingPrice")
                        .HasColumnType("decimal(9, 2)");

                    b.Property<decimal>("D")
                        .HasColumnType("decimal(9, 3)");

                    b.Property<decimal>("Dea")
                        .HasColumnType("decimal(9, 3)");

                    b.Property<decimal>("Dif")
                        .HasColumnType("decimal(9, 3)");

                    b.Property<decimal>("Ema12")
                        .HasColumnType("decimal(9, 3)");

                    b.Property<decimal>("Ema26")
                        .HasColumnType("decimal(9, 3)");

                    b.Property<decimal>("HighestPrice")
                        .HasColumnType("decimal(9, 2)");

                    b.Property<decimal>("K")
                        .HasColumnType("decimal(9, 3)");

                    b.Property<decimal>("LowestPrice")
                        .HasColumnType("decimal(9, 2)");

                    b.Property<decimal>("OpeningPrice")
                        .HasColumnType("decimal(9, 2)");

                    b.Property<decimal>("Osc")
                        .HasColumnType("decimal(9, 3)");

                    b.Property<long>("TradeValue")
                        .HasColumnType("bigint");

                    b.Property<int>("Transaction")
                        .HasColumnType("int");

                    b.HasKey("StockNo", "Date");

                    b.ToTable("StockWeek");
                });
#pragma warning restore 612, 618
        }
    }
}
