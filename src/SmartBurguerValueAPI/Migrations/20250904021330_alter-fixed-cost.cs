using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBurguerValueAPI.Migrations
{
    /// <inheritdoc />
    public partial class alterfixedcost : Migration
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
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                collation: "ascii_general_ci",
                oldClrType: typeof(Guid),
                oldType: "char(36)",
                oldNullable: true)
                .OldAnnotation("Relational:Collation", "ascii_general_ci");

            migrationBuilder.AddColumn<DateTime>(
                name: "DueDate",
                table: "FixedCosts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "FixedCosts",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "PaymentDate",
                table: "FixedCosts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

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

            migrationBuilder.DropColumn(
                name: "DueDate",
                table: "FixedCosts");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "FixedCosts");

            migrationBuilder.DropColumn(
                name: "PaymentDate",
                table: "FixedCosts");

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
    }
}
