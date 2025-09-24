using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBurguerValueAPI.Migrations
{
    /// <inheritdoc />
    public partial class addtotalorders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "TotalOrders",
                table: "DailyEntry",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalOrders",
                table: "DailyEntry");
        }
    }
}
