using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Billing.Infrastructure.Persistence.Migrations
{
    public partial class AddingAccountHolder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "billing",
                table: "Accounts");

            migrationBuilder.AddColumn<int>(
                name: "AccountHolderId",
                schema: "billing",
                table: "Accounts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "AccountHolders",
                schema: "billing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccountHolders", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Accounts_AccountHolderId",
                schema: "billing",
                table: "Accounts",
                column: "AccountHolderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Accounts_AccountHolders_AccountHolderId",
                schema: "billing",
                table: "Accounts",
                column: "AccountHolderId",
                principalSchema: "billing",
                principalTable: "AccountHolders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Accounts_AccountHolders_AccountHolderId",
                schema: "billing",
                table: "Accounts");

            migrationBuilder.DropTable(
                name: "AccountHolders",
                schema: "billing");

            migrationBuilder.DropIndex(
                name: "IX_Accounts_AccountHolderId",
                schema: "billing",
                table: "Accounts");

            migrationBuilder.DropColumn(
                name: "AccountHolderId",
                schema: "billing",
                table: "Accounts");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "billing",
                table: "Accounts",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
