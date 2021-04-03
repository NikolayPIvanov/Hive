using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Gig.Infrastructure.Persistence.Migrations
{
    public partial class AddingPackagesToGigEnitity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_GigScopes_GigScopeId",
                schema: "gmt",
                table: "Packages");

            migrationBuilder.RenameColumn(
                name: "GigScopeId",
                schema: "gmt",
                table: "Packages",
                newName: "GigId");

            migrationBuilder.RenameIndex(
                name: "IX_Packages_GigScopeId",
                schema: "gmt",
                table: "Packages",
                newName: "IX_Packages_GigId");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_Gigs_GigId",
                schema: "gmt",
                table: "Packages",
                column: "GigId",
                principalSchema: "gmt",
                principalTable: "Gigs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Packages_Gigs_GigId",
                schema: "gmt",
                table: "Packages");

            migrationBuilder.RenameColumn(
                name: "GigId",
                schema: "gmt",
                table: "Packages",
                newName: "GigScopeId");

            migrationBuilder.RenameIndex(
                name: "IX_Packages_GigId",
                schema: "gmt",
                table: "Packages",
                newName: "IX_Packages_GigScopeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Packages_GigScopes_GigScopeId",
                schema: "gmt",
                table: "Packages",
                column: "GigScopeId",
                principalSchema: "gmt",
                principalTable: "GigScopes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
