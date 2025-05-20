using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBurguerValueAPI.Migrations
{
    /// <inheritdoc />
    public partial class salesgoal : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FixedCostEntity_Enterprise_EnterpriseId",
                table: "FixedCostEntity");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FixedCostEntity",
                table: "FixedCostEntity");

            migrationBuilder.RenameTable(
                name: "FixedCostEntity",
                newName: "FixedCosts");

            migrationBuilder.RenameIndex(
                name: "IX_FixedCostEntity_EnterpriseId",
                table: "FixedCosts",
                newName: "IX_FixedCosts_EnterpriseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FixedCosts",
                table: "FixedCosts",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "SalesGoal",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    Description = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    GoalValue = table.Column<string>(type: "longtext", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    StartDate = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EndDate = table.Column<string>(type: "longtext", nullable: true)
                        .Annotation("MySql:CharSet", "utf8mb4"),
                    EnterpriseId = table.Column<Guid>(type: "char(36)", nullable: false, collation: "ascii_general_ci"),
                    DateCreated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    DateUpdated = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    IsActive = table.Column<bool>(type: "tinyint(1)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesGoal", x => x.Id);
                })
                .Annotation("MySql:CharSet", "utf8mb4");

            migrationBuilder.AddForeignKey(
                name: "FK_FixedCosts_Enterprise_EnterpriseId",
                table: "FixedCosts",
                column: "EnterpriseId",
                principalTable: "Enterprise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FixedCosts_Enterprise_EnterpriseId",
                table: "FixedCosts");

            migrationBuilder.DropTable(
                name: "SalesGoal");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FixedCosts",
                table: "FixedCosts");

            migrationBuilder.RenameTable(
                name: "FixedCosts",
                newName: "FixedCostEntity");

            migrationBuilder.RenameIndex(
                name: "IX_FixedCosts_EnterpriseId",
                table: "FixedCostEntity",
                newName: "IX_FixedCostEntity_EnterpriseId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FixedCostEntity",
                table: "FixedCostEntity",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FixedCostEntity_Enterprise_EnterpriseId",
                table: "FixedCostEntity",
                column: "EnterpriseId",
                principalTable: "Enterprise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
