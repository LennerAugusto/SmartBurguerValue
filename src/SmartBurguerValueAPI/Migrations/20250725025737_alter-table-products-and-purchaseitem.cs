using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBurguerValueAPI.Migrations
{
    /// <inheritdoc />
    public partial class altertableproductsandpurchaseitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "IngredientId",
                table: "PurchaseItem",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<decimal>(
                name: "CMV",
                table: "Products",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CPV",
                table: "Products",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "DesiredMargin",
                table: "Products",
                type: "decimal(65,30)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "SuggestedPrice",
                table: "Products",
                type: "decimal(65,30)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IngredientId",
                table: "PurchaseItem");

            migrationBuilder.DropColumn(
                name: "CMV",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "CPV",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "DesiredMargin",
                table: "Products");

            migrationBuilder.DropColumn(
                name: "SuggestedPrice",
                table: "Products");
        }
    }
}
