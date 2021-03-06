using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Infrastructure.Persistence.Migrations
{
    public partial class Foreignkeysforselleranduserprofile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sellers_UserProfiles_UserProfileId",
                table: "Sellers");

            migrationBuilder.DropIndex(
                name: "IX_Sellers_UserProfileId",
                table: "Sellers");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_SellerId",
                table: "UserProfiles",
                column: "SellerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProfiles_Sellers_SellerId",
                table: "UserProfiles",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserProfiles_Sellers_SellerId",
                table: "UserProfiles");

            migrationBuilder.DropIndex(
                name: "IX_UserProfiles_SellerId",
                table: "UserProfiles");

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_UserProfileId",
                table: "Sellers",
                column: "UserProfileId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Sellers_UserProfiles_UserProfileId",
                table: "Sellers",
                column: "UserProfileId",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
