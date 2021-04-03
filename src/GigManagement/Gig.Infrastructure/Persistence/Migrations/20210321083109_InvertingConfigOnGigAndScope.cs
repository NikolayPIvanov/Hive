using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Gig.Infrastructure.Persistence.Migrations
{
    public partial class InvertingConfigOnGigAndScope : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GigScope_Gigs_GigId",
                schema: "gmt",
                table: "GigScope");

            migrationBuilder.DropIndex(
                name: "IX_GigScope_GigId",
                schema: "gmt",
                table: "GigScope");

            migrationBuilder.CreateIndex(
                name: "IX_Gigs_GigScopeId",
                schema: "gmt",
                table: "Gigs",
                column: "GigScopeId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Gigs_GigScope_GigScopeId",
                schema: "gmt",
                table: "Gigs",
                column: "GigScopeId",
                principalSchema: "gmt",
                principalTable: "GigScope",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gigs_GigScope_GigScopeId",
                schema: "gmt",
                table: "Gigs");

            migrationBuilder.DropIndex(
                name: "IX_Gigs_GigScopeId",
                schema: "gmt",
                table: "Gigs");

            migrationBuilder.CreateIndex(
                name: "IX_GigScope_GigId",
                schema: "gmt",
                table: "GigScope",
                column: "GigId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GigScope_Gigs_GigId",
                schema: "gmt",
                table: "GigScope",
                column: "GigId",
                principalSchema: "gmt",
                principalTable: "Gigs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
