using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBurguerValueAPI.Migrations
{
    /// <inheritdoc />
    public partial class alterunityofmeansurepurchase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseItem_UnitytypesProducts_UnityOfMensureId",
                table: "PurchaseItem");

            migrationBuilder.AlterColumn<Guid>(
                name: "UnityOfMensureId",
                table: "PurchaseItem",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)")
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseItem_UnitytypesProducts_UnityOfMensureId",
                table: "PurchaseItem",
                column: "UnityOfMensureId",
                principalTable: "UnitytypesProducts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PurchaseItem_UnitytypesProducts_UnityOfMensureId",
                table: "PurchaseItem");

            migrationBuilder.AlterColumn<Guid>(
                name: "UnityOfMensureId",
                table: "PurchaseItem",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddForeignKey(
                name: "FK_PurchaseItem_UnitytypesProducts_UnityOfMensureId",
                table: "PurchaseItem",
                column: "UnityOfMensureId",
                principalTable: "UnitytypesProducts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
