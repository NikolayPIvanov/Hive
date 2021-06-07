using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Gig.Infrastructure.Persistence.Migrations
{
    public partial class GigImages : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Categories_ParentId1",
                schema: "gmt",
                table: "Categories");

            migrationBuilder.DropIndex(
                name: "IX_Categories_ParentId1",
                schema: "gmt",
                table: "Categories");

            migrationBuilder.DropColumn(
                name: "ParentId1",
                schema: "gmt",
                table: "Categories");

            migrationBuilder.CreateTable(
                name: "GigsImages",
                schema: "gmt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Path = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    GigId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GigsImages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GigsImages_Gigs_GigId",
                        column: x => x.GigId,
                        principalSchema: "gmt",
                        principalTable: "Gigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GigsImages_GigId",
                schema: "gmt",
                table: "GigsImages",
                column: "GigId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GigsImages",
                schema: "gmt");

            migrationBuilder.AddColumn<int>(
                name: "ParentId1",
                schema: "gmt",
                table: "Categories",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Categories_ParentId1",
                schema: "gmt",
                table: "Categories",
                column: "ParentId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Categories_ParentId1",
                schema: "gmt",
                table: "Categories",
                column: "ParentId1",
                principalSchema: "gmt",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
