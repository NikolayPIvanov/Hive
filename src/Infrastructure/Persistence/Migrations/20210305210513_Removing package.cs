using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Infrastructure.Persistence.Migrations
{
    public partial class Removingpackage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Gigs_GigId",
                table: "Packages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Packages",
                table: "Packages");

            migrationBuilder.RenameTable(
                name: "Packages",
                newName: "Package");

            migrationBuilder.RenameIndex(
                name: "IX_Packages_GigId",
                table: "Package",
                newName: "IX_Package_GigId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Package",
                table: "Package",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Package_Gigs_GigId",
                table: "Package",
                column: "GigId",
                principalTable: "Gigs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Package_Gigs_GigId",
                table: "Package");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Package",
                table: "Package");

            migrationBuilder.RenameTable(
                name: "Package",
                newName: "Packages");

            migrationBuilder.RenameIndex(
                name: "IX_Package_GigId",
                table: "Packages",
                newName: "IX_Packages_GigId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Packages",
                table: "Packages",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Gigs_GigId",
                table: "Packages",
                column: "GigId",
                principalTable: "Gigs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
