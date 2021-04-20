using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Infrastructure.Persistence.Migrations
{
    public partial class AddingAccountHolderCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BillingAccountId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_AccountHolders_BillingAccountId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_BillingAccountId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "BillingAccountId",
                table: "AspNetUsers");
        }
    }
}
