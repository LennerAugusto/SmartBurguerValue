using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBurguerValueAPI.Migrations
{
    /// <inheritdoc />
    public partial class alterpurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NameItem",
                table: "PurchaseItem",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "UnityOfMensureId",
                table: "PurchaseItem",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "PurchaseDate",
                table: "Purchase",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "EnterpriseId",
                table: "InventoryItem",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseItem_UnityOfMensureId",
                table: "PurchaseItem",
                column: "UnityOfMensureId");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseItem_UnitytypesProducts_UnityOfMensureId",
                table: "PurchaseItem",
                column: "UnityOfMensureId",
                principalTable: "UnitytypesProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseItem_UnitytypesProducts_UnityOfMensureId",
                table: "PurchaseItem");

            migrationBuilder.DropIndex(
                name: "IX_PurchaseItem_UnityOfMensureId",
                table: "PurchaseItem");

            migrationBuilder.DropColumn(
                name: "NameItem",
                table: "PurchaseItem");

            migrationBuilder.DropColumn(
                name: "UnityOfMensureId",
                table: "PurchaseItem");

            migrationBuilder.DropColumn(
                name: "PurchaseDate",
                table: "Purchase");

            migrationBuilder.DropColumn(
                name: "EnterpriseId",
                table: "InventoryItem");
        }
    }
}
