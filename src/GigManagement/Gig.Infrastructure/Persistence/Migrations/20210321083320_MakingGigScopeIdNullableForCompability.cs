using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Gig.Infrastructure.Persistence.Migrations
{
    public partial class MakingGigScopeIdNullableForCompability : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gigs_GigScope_GigScopeId",
                schema: "gmt",
                table: "Gigs");

            migrationBuilder.DropIndex(
                name: "IX_Gigs_GigScopeId",
                schema: "gmt",
                table: "Gigs");

            migrationBuilder.AlterColumn<int>(
                name: "GigScopeId",
                schema: "gmt",
                table: "Gigs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Gigs_GigScopeId",
                schema: "gmt",
                table: "Gigs",
                column: "GigScopeId",
                unique: true,
                filter: "[GigScopeId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Gigs_GigScope_GigScopeId",
                schema: "gmt",
                table: "Gigs",
                column: "GigScopeId",
                principalSchema: "gmt",
                principalTable: "GigScope",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
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

            migrationBuilder.AlterColumn<int>(
                name: "GigScopeId",
                schema: "gmt",
                table: "Gigs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

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
    }
}
