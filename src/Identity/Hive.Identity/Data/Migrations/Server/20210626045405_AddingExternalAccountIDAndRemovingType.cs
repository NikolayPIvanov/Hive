using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Identity.Data.Migrations.Server
{
    public partial class AddingExternalAccountIDAndRemovingType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserAccountTypes_AccountTypeId",
                table: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "UserAccountTypes");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AccountTypeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AccountTypeId",
                table: "AspNetUsers");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AccountTypeId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "UserAccountTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAccountTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AccountTypeId",
                table: "AspNetUsers",
                column: "AccountTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountTypes_Type_UserId",
                table: "UserAccountTypes",
                columns: new[] { "Type", "UserId" },
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserAccountTypes_AccountTypeId",
                table: "AspNetUsers",
                column: "AccountTypeId",
                principalTable: "UserAccountTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
