using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBurguerValueAPI.Migrations
{
    /// <inheritdoc />
    public partial class categoryproducts2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsAcive",
                table: "CategoryProducts",
                newName: "IsActive");

            migrationBuilder.AddColumn<DateTime>(
                name: "DateCreated",
                table: "CategoryProducts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DateUpdated",
                table: "CategoryProducts",
                type: "datetime(6)",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DateCreated",
                table: "CategoryProducts");

            migrationBuilder.DropColumn(
                name: "DateUpdated",
                table: "CategoryProducts");

            migrationBuilder.RenameColumn(
                name: "IsActive",
                table: "CategoryProducts",
                newName: "IsAcive");
        }
    }
}
