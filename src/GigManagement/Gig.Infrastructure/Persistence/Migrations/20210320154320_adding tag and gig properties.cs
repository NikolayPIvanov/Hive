using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Gig.Infrastructure.Persistence.Migrations
{
    public partial class addingtagandgigproperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Description",
                schema: "gmt",
                table: "Gigs");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedBy",
                schema: "gmt",
                table: "Gigs",
                type: "nvarchar(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                schema: "gmt",
                table: "Gigs",
                type: "datetime2(2)",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "gmt",
                table: "Gigs",
                type: "nvarchar(36)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                schema: "gmt",
                table: "Gigs",
                type: "datetime2(2)",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<bool>(
                name: "IsDraft",
                schema: "gmt",
                table: "Gigs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "Tag",
                schema: "gmt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                });

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
                        name: "FK_GigTag_Tag_TagsId",
                        column: x => x.TagsId,
                        principalSchema: "gmt",
                        principalTable: "Tag",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gigs_CategoryId",
                schema: "gmt",
                table: "Gigs",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_GigTag_TagsId",
                schema: "gmt",
                table: "GigTag",
                column: "TagsId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gigs_Categories_CategoryId",
                schema: "gmt",
                table: "Gigs",
                column: "CategoryId",
                principalSchema: "gmt",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gigs_Categories_CategoryId",
                schema: "gmt",
                table: "Gigs");

            migrationBuilder.DropTable(
                name: "GigTag",
                schema: "gmt");

            migrationBuilder.DropTable(
                name: "Tag",
                schema: "gmt");

            migrationBuilder.DropIndex(
                name: "IX_Gigs_CategoryId",
                schema: "gmt",
                table: "Gigs");

            migrationBuilder.DropColumn(
                name: "IsDraft",
                schema: "gmt",
                table: "Gigs");

            migrationBuilder.AlterColumn<string>(
                name: "LastModifiedBy",
                schema: "gmt",
                table: "Gigs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(36)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "LastModified",
                schema: "gmt",
                table: "Gigs",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                schema: "gmt",
                table: "Gigs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(36)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Created",
                schema: "gmt",
                table: "Gigs",
                type: "datetime2",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldType: "datetime2(2)");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                schema: "gmt",
                table: "Gigs",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: false,
                defaultValue: "");
        }
    }
}
