using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BX_Stock.Migrations
{
    public partial class init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Stock",
                columns: table => new
                {
                    StockNo = table.Column<int>(unicode: false, fixedLength: true, maxLength: 4, nullable: false),
                    StockName = table.Column<string>(fixedLength: true, maxLength: 10, nullable: false),
                    IsListed = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stock", x => x.StockNo);
                });

            migrationBuilder.CreateTable(
                name: "StockDay",
                columns: table => new
                {
                    StockNo = table.Column<int>(fixedLength: true, maxLength: 4, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    TradeValue = table.Column<long>(nullable: false),
                    OpeningPrice = table.Column<decimal>(type: "decimal(9, 2)", nullable: false),
                    HighestPrice = table.Column<decimal>(type: "decimal(9, 2)", nullable: false),
                    LowestPrice = table.Column<decimal>(type: "decimal(9, 2)", nullable: false),
                    ClosingPrice = table.Column<decimal>(type: "decimal(9, 2)", nullable: false),
                    Change = table.Column<decimal>(type: "decimal(9, 2)", nullable: false),
                    Transaction = table.Column<int>(nullable: false),
                    K = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    D = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    Ema12 = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    Ema26 = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    Dif = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    Dea = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    Osc = table.Column<decimal>(type: "decimal(9, 3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DayStock", x => new { x.StockNo, x.Date });
                });

            migrationBuilder.CreateTable(
                name: "StockMonth",
                columns: table => new
                {
                    StockNo = table.Column<int>(unicode: false, fixedLength: true, maxLength: 4, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    TradeValue = table.Column<long>(nullable: false),
                    OpeningPrice = table.Column<decimal>(type: "decimal(9, 2)", nullable: false),
                    HighestPrice = table.Column<decimal>(type: "decimal(9, 2)", nullable: false),
                    LowestPrice = table.Column<decimal>(type: "decimal(9, 2)", nullable: false),
                    ClosingPrice = table.Column<decimal>(type: "decimal(9, 2)", nullable: false),
                    Change = table.Column<decimal>(type: "decimal(9, 2)", nullable: false),
                    Transaction = table.Column<int>(nullable: false),
                    K = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    D = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    Ema12 = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    Ema26 = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    Dif = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    Dea = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    Osc = table.Column<decimal>(type: "decimal(9, 3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockMonth", x => new { x.StockNo, x.Date });
                });

            migrationBuilder.CreateTable(
                name: "StockWeek",
                columns: table => new
                {
                    StockNo = table.Column<int>(unicode: false, fixedLength: true, maxLength: 4, nullable: false),
                    Date = table.Column<DateTime>(type: "datetime", nullable: false),
                    TradeValue = table.Column<long>(nullable: false),
                    OpeningPrice = table.Column<decimal>(type: "decimal(9, 2)", nullable: false),
                    HighestPrice = table.Column<decimal>(type: "decimal(9, 2)", nullable: false),
                    LowestPrice = table.Column<decimal>(type: "decimal(9, 2)", nullable: false),
                    ClosingPrice = table.Column<decimal>(type: "decimal(9, 2)", nullable: false),
                    Change = table.Column<decimal>(type: "decimal(9, 2)", nullable: false),
                    Transaction = table.Column<int>(nullable: false),
                    K = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    D = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    Ema12 = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    Ema26 = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    Dif = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    Dea = table.Column<decimal>(type: "decimal(9, 3)", nullable: false),
                    Osc = table.Column<decimal>(type: "decimal(9, 3)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockWeek", x => new { x.StockNo, x.Date });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Stock");

            migrationBuilder.DropTable(
                name: "StockDay");

            migrationBuilder.DropTable(
                name: "StockMonth");

            migrationBuilder.DropTable(
                name: "StockWeek");
        }
    }
}
