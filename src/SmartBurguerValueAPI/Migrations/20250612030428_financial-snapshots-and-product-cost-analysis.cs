using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBurguerValueAPI.Migrations
{
    /// <inheritdoc />
    public partial class financialsnapshotsandproductcostanalysis : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Revenue",
                table: "DailyEntry");

            migrationBuilder.AddColumn<decimal>(
                name: "SellingPrice",
                table: "Products",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SellingPrice",
                table: "Combos",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateTable(
                name: "DailyEntryItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DailyEntryId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProductId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    ComboId = table.Column<Guid>(type: "char(36)", nullable: true, collation: "ascii_general_ci"),
                    Quantity = table.Column<int>(type: "int", nullable: false),
                    SellingPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    CPV = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    TotalRevenue = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    TotalCPV = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DailyEntryItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DailyEntryItem_Combos_ComboId",
                        column: x => x.ComboId,
                        principalTable: "Combos",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_DailyEntryItem_DailyEntry_DailyEntryId",
                        column: x => x.DailyEntryId,
                        principalTable: "DailyEntry",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DailyEntryItem_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id");
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "FinancialSnapshots",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EnterpriseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    SnapshotDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    TotalRevenue = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    TotalCost = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    GrossProfit = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Markup = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    Margin = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    CPV = table.Column<decimal>(type: "decimal(65,30)", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FinancialSnapshots", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FinancialSnapshots_Enterprise_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateTable(
                name: "ProductCostAnalysis",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    ProductId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    EnterpriseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    AnalisysDate = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    UnitCost = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    SellingPrice = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    SellingPriceSuggested = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Markup = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    Margin = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    CPV = table.Column<decimal>(type: "decimal(65,30)", nullable: false),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProductCostAnalysis", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProductCostAnalysis_Enterprise_EnterpriseId",
                        column: x => x.EnterpriseId,
                        principalTable: "Enterprise",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProductCostAnalysis_Products_ProductId",
                        column: x => x.ProductId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.CreateIndex(
                name: "IX_DailyEntryItem_ComboId",
                table: "DailyEntryItem",
                column: "ComboId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyEntryItem_DailyEntryId",
                table: "DailyEntryItem",
                column: "DailyEntryId");

            migrationBuilder.CreateIndex(
                name: "IX_DailyEntryItem_ProductId",
                table: "DailyEntryItem",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_FinancialSnapshots_EnterpriseId",
                table: "FinancialSnapshots",
                column: "EnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCostAnalysis_EnterpriseId",
                table: "ProductCostAnalysis",
                column: "EnterpriseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProductCostAnalysis_ProductId",
                table: "ProductCostAnalysis",
                column: "ProductId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DailyEntryItem");

            migrationBuilder.DropTable(
                name: "FinancialSnapshots");

            migrationBuilder.DropTable(
                name: "ProductCostAnalysis");

            migrationBuilder.DropColumn(
                name: "SellingPrice",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SellingPrice",
                table: "Combos");

            migrationBuilder.AddColumn<string>(
                name: "Revenue",
                table: "DailyEntry",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }
    }
}
