using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBurguerValueAPI.Migrations
{
    /// <inheritdoc />
    public partial class addcolumnempoloyee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "HiringDate",
                table: "Employees",
                type: "datetime(6)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HiringDate",
                table: "Employees");
        }
    }
}
