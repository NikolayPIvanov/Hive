using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Gig.Infrastructure.Persistence.Migrations
{
    public partial class UpdatingEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "GigTag",
                schema: "gmt");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                schema: "gmt",
                table: "Tags",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AddColumn<int>(
                name: "GigId",
                schema: "gmt",
                table: "Tags",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "RevisionType",
                schema: "gmt",
                table: "Packages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Tags_GigId",
                schema: "gmt",
                table: "Tags",
                column: "GigId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tags_Gigs_GigId",
                schema: "gmt",
                table: "Tags",
                column: "GigId",
                principalSchema: "gmt",
                principalTable: "Gigs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tags_Gigs_GigId",
                schema: "gmt",
                table: "Tags");

            migrationBuilder.DropIndex(
                name: "IX_Tags_GigId",
                schema: "gmt",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "GigId",
                schema: "gmt",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "RevisionType",
                schema: "gmt",
                table: "Packages");

            migrationBuilder.AlterColumn<string>(
                name: "Value",
                schema: "gmt",
                table: "Tags",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "GigTag",
                schema: "gmt",
                columns: table => new
                {
                    GigsId = table.Column<int>(type: "int", nullable: false),
                    TagsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GigTag", x => new { x.GigsId, x.TagsId });
                    table.ForeignKey(
                        name: "FK_GigTag_Gigs_GigsId",
                        column: x => x.GigsId,
                        principalSchema: "gmt",
                        principalTable: "Gigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_GigTag_Tags_TagsId",
                        column: x => x.TagsId,
                        principalSchema: "gmt",
                        principalTable: "Tags",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GigTag_TagsId",
                schema: "gmt",
                table: "GigTag",
                column: "TagsId");
        }
    }
}
