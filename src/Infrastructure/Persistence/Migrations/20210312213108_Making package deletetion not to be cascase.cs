using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Infrastructure.Persistence.Migrations
{
    public partial class Makingpackagedeletetionnottobecascase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Packages_PackageId",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Packages_PackageId",
                table: "Orders",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Packages_PackageId",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Packages_PackageId",
                table: "Orders",
                column: "PackageId",
                principalTable: "Packages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
