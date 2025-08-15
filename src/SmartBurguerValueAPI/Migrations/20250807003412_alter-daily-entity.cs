using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBurguerValueAPI.Migrations
{
    /// <inheritdoc />
    public partial class alterdailyentity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CPV",
                table: "DailyEntryItem",
                newName: "DessiredMargin");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "DailyEntryItem",
                type: "longtext",
                nullable: false)
                .Annotation("MySql:CharSet", "utf8mb4");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Name",
                table: "DailyEntryItem");

            migrationBuilder.RenameColumn(
                name: "DessiredMargin",
                table: "DailyEntryItem",
                newName: "CPV");
        }
    }
}
