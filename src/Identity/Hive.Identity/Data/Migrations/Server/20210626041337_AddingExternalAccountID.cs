using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Identity.Data.Migrations.Server
{
    public partial class AddingExternalAccountID : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserAccountTypes_AspNetUsers_UserId",
                table: "UserAccountTypes");

            migrationBuilder.DropIndex(
                name: "IX_UserAccountTypes_UserId",
                table: "UserAccountTypes");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "InvestorId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "SellerId",
                table: "AspNetUsers",
                newName: "ExternalAccountId");

            migrationBuilder.AddColumn<int>(
                name: "AccountTypeId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_AccountTypeId",
                table: "AspNetUsers",
                column: "AccountTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_UserAccountTypes_AccountTypeId",
                table: "AspNetUsers",
                column: "AccountTypeId",
                principalTable: "UserAccountTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_UserAccountTypes_AccountTypeId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_AccountTypeId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "AccountTypeId",
                table: "AspNetUsers");

            migrationBuilder.RenameColumn(
                name: "ExternalAccountId",
                table: "AspNetUsers",
                newName: "SellerId");

            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvestorId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserAccountTypes_UserId",
                table: "UserAccountTypes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserAccountTypes_AspNetUsers_UserId",
                table: "UserAccountTypes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
