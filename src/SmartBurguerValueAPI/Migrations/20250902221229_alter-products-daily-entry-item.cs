using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBurguerValueAPI.Migrations
{
    /// <inheritdoc />
    public partial class alterproductsdailyentryitem : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyEntryItem_Products_ProductId",
                table: "DailyEntryItem");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Employees");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyEntryItem_Products_ProductId",
                table: "DailyEntryItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_DailyEntryItem_Products_ProductId",
                table: "DailyEntryItem");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Employees",
                type: "longtext",
                nullable: true)
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_DailyEntryItem_Products_ProductId",
                table: "DailyEntryItem",
                column: "ProductId",
                principalTable: "Products",
                principalColumn: "Id");
        }
    }
}
