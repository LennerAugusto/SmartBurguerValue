using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBurguerValueAPI.Migrations
{
    /// <inheritdoc />
    public partial class alterusersenterpriseid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InventoryItem_Ingredients_IngredientId",
                table: "InventoryItem");

            migrationBuilder.DropIndex(
                name: "IX_InventoryItem_IngredientId",
                table: "InventoryItem");

            migrationBuilder.DropColumn(
                name: "IngredientId",
                table: "InventoryItem");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "PurchasePrice",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "PurchaseQuantity",
                table: "Ingredients");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitCost",
                table: "ProductCostAnalysis",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingPriceSuggested",
                table: "ProductCostAnalysis",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingPrice",
                table: "ProductCostAnalysis",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Markup",
                table: "ProductCostAnalysis",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Margin",
                table: "ProductCostAnalysis",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AlterColumn<decimal>(
                name: "CPV",
                table: "ProductCostAnalysis",
                type: "decimal(65,30)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)");

            migrationBuilder.AddColumn<Guid>(
                name: "EnterpriseId",
                table: "AspNetUsers",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_Ingredients_InventoryItemId",
                table: "Ingredients",
                column: "InventoryItemId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ingredients_InventoryItem_InventoryItemId",
                table: "Ingredients",
                column: "InventoryItemId",
                principalTable: "InventoryItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ingredients_InventoryItem_InventoryItemId",
                table: "Ingredients");

            migrationBuilder.DropIndex(
                name: "IX_Ingredients_InventoryItemId",
                table: "Ingredients");

            migrationBuilder.DropColumn(
                name: "EnterpriseId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<decimal>(
                name: "UnitCost",
                table: "ProductCostAnalysis",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingPriceSuggested",
                table: "ProductCostAnalysis",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "SellingPrice",
                table: "ProductCostAnalysis",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Markup",
                table: "ProductCostAnalysis",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Margin",
                table: "ProductCostAnalysis",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "CPV",
                table: "ProductCostAnalysis",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m,
                oldClrType: typeof(decimal),
                oldType: "decimal(65,30)",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "IngredientId",
                table: "InventoryItem",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "Ingredients",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<decimal>(
                name: "PurchasePrice",
                table: "Ingredients",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "PurchaseQuantity",
                table: "Ingredients",
                type: "decimal(65,30)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_InventoryItem_IngredientId",
                table: "InventoryItem",
                column: "IngredientId");

            migrationBuilder.AddForeignKey(
                name: "FK_InventoryItem_Ingredients_IngredientId",
                table: "InventoryItem",
                column: "IngredientId",
                principalTable: "Ingredients",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
