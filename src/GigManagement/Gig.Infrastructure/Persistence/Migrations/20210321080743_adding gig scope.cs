using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Gig.Infrastructure.Persistence.Migrations
{
    public partial class addinggigscope : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GigTag_Tag_TagsId",
                schema: "gmt",
                table: "GigTag");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tag",
                schema: "gmt",
                table: "Tag");

            migrationBuilder.RenameTable(
                name: "Tag",
                schema: "gmt",
                newName: "Tags",
                newSchema: "gmt");

            migrationBuilder.AddColumn<int>(
                name: "GigScopeId",
                schema: "gmt",
                table: "Gigs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tags",
                schema: "gmt",
                table: "Tags",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "GigScope",
                schema: "gmt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Description = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: false),
                    GigId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2(2)", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(36)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2(2)", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_GigScope", x => x.Id);
                    table.ForeignKey(
                        name: "FK_GigScope_Gigs_GigId",
                        column: x => x.GigId,
                        principalSchema: "gmt",
                        principalTable: "Gigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_GigScope_GigId",
                schema: "gmt",
                table: "GigScope",
                column: "GigId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_GigTag_Tags_TagsId",
                schema: "gmt",
                table: "GigTag",
                column: "TagsId",
                principalSchema: "gmt",
                principalTable: "Tags",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GigTag_Tags_TagsId",
                schema: "gmt",
                table: "GigTag");

            migrationBuilder.DropTable(
                name: "GigScope",
                schema: "gmt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Tags",
                schema: "gmt",
                table: "Tags");

            migrationBuilder.DropColumn(
                name: "GigScopeId",
                schema: "gmt",
                table: "Gigs");

            migrationBuilder.RenameTable(
                name: "Tags",
                schema: "gmt",
                newName: "Tag",
                newSchema: "gmt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Tag",
                schema: "gmt",
                table: "Tag",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_GigTag_Tag_TagsId",
                schema: "gmt",
                table: "GigTag",
                column: "TagsId",
                principalSchema: "gmt",
                principalTable: "Tag",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
