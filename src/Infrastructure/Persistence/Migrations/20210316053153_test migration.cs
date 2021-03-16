using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Infrastructure.Persistence.Migrations
{
    public partial class testmigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Review_Gigs_GigId",
                table: "Review");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Review",
                table: "Review");

            migrationBuilder.RenameTable(
                name: "Review",
                newName: "Reviews");

            migrationBuilder.RenameIndex(
                name: "IX_Review_GigId",
                table: "Reviews",
                newName: "IX_Reviews_GigId");

            migrationBuilder.AddColumn<int>(
                name: "CategoryId1",
                table: "Gigs",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Gigs_CategoryId1",
                table: "Gigs",
                column: "CategoryId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Gigs_Categories_CategoryId1",
                table: "Gigs",
                column: "CategoryId1",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Gigs_GigId",
                table: "Reviews",
                column: "GigId",
                principalTable: "Gigs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gigs_Categories_CategoryId1",
                table: "Gigs");

            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Gigs_GigId",
                table: "Reviews");

            migrationBuilder.DropIndex(
                name: "IX_Gigs_CategoryId1",
                table: "Gigs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Reviews",
                table: "Reviews");

            migrationBuilder.DropColumn(
                name: "CategoryId1",
                table: "Gigs");

            migrationBuilder.RenameTable(
                name: "Reviews",
                newName: "Review");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_GigId",
                table: "Review",
                newName: "IX_Review_GigId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Review",
                table: "Review",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Review_Gigs_GigId",
                table: "Review",
                column: "GigId",
                principalTable: "Gigs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
