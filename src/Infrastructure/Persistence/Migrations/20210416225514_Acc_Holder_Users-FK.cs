using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Infrastructure.Persistence.Migrations
{
    public partial class Acc_Holder_UsersFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AccountHolders_BillingAccountId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BillingAccountId",
                table: "AspNetUsers");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AccountHolders",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountHolders_UserId",
                table: "AccountHolders",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountHolders_AspNetUsers_UserId",
                table: "AccountHolders",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountHolders_AspNetUsers_UserId",
                table: "AccountHolders");

            migrationBuilder.DropIndex(
                name: "IX_AccountHolders_UserId",
                table: "AccountHolders");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "AccountHolders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_BillingAccountId",
                table: "AspNetUsers",
                column: "BillingAccountId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_AccountHolders_BillingAccountId",
                table: "AspNetUsers",
                column: "BillingAccountId",
                principalTable: "AccountHolders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
