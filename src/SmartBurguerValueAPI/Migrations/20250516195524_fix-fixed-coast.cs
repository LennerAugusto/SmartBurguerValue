using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartBurguerValueAPI.Migrations
{
    /// <inheritdoc />
    public partial class fixfixedcoast : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FixedCostEntity_Enterprise_EnterpriseEntityId",
                table: "FixedCostEntity");

            migrationBuilder.DropIndex(
                name: "IX_FixedCostEntity_EnterpriseEntityId",
                table: "FixedCostEntity");

            migrationBuilder.DropColumn(
                name: "EnterpriseEntityId",
                table: "FixedCostEntity");

            migrationBuilder.CreateIndex(
                name: "IX_FixedCostEntity_EnterpriseId",
                table: "FixedCostEntity",
                column: "EnterpriseId");

            migrationBuilder.AddForeignKey(
                name: "FK_FixedCostEntity_Enterprise_EnterpriseId",
                table: "FixedCostEntity",
                column: "EnterpriseId",
                principalTable: "Enterprise",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FixedCostEntity_Enterprise_EnterpriseId",
                table: "FixedCostEntity");

            migrationBuilder.DropIndex(
                name: "IX_FixedCostEntity_EnterpriseId",
                table: "FixedCostEntity");

            migrationBuilder.AddColumn<Guid>(
                name: "EnterpriseEntityId",
                table: "FixedCostEntity",
                type: "char(36)",
                nullable: true,
                collation: "ascii_general_ci");

            migrationBuilder.CreateIndex(
                name: "IX_FixedCostEntity_EnterpriseEntityId",
                table: "FixedCostEntity",
                column: "EnterpriseEntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_FixedCostEntity_Enterprise_EnterpriseEntityId",
                table: "FixedCostEntity",
                column: "EnterpriseEntityId",
                principalTable: "Enterprise",
                principalColumn: "Id");
        }
    }
}
