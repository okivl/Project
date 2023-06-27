using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Project.Api.Migrations
{
    /// <inheritdoc />
    public partial class IncomeSourcesFix : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "IncomeSources",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "ExpenseTypes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_IncomeSources_UserId",
                table: "IncomeSources",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ExpenseTypes_UserId",
                table: "ExpenseTypes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExpenseTypes_Users_UserId",
                table: "ExpenseTypes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_IncomeSources_Users_UserId",
                table: "IncomeSources",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExpenseTypes_Users_UserId",
                table: "ExpenseTypes");

            migrationBuilder.DropForeignKey(
                name: "FK_IncomeSources_Users_UserId",
                table: "IncomeSources");

            migrationBuilder.DropIndex(
                name: "IX_IncomeSources_UserId",
                table: "IncomeSources");

            migrationBuilder.DropIndex(
                name: "IX_ExpenseTypes_UserId",
                table: "ExpenseTypes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "IncomeSources");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "ExpenseTypes");
        }
    }
}
