using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Gig.Infrastructure.Persistence.Migrations
{
    public partial class AddingPackagesEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gigs_GigScope_GigScopeId",
                schema: "gmt",
                table: "Gigs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GigScope",
                schema: "gmt",
                table: "GigScope");

            migrationBuilder.RenameTable(
                name: "GigScope",
                schema: "gmt",
                newName: "GigScopes",
                newSchema: "gmt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GigScopes",
                schema: "gmt",
                table: "GigScopes",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Packages",
                schema: "gmt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PackageTier = table.Column<int>(type: "int", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    DeliveryTime = table.Column<double>(type: "float", nullable: false),
                    DeliveryFrequency = table.Column<int>(type: "int", nullable: false),
                    Revisions = table.Column<int>(type: "int", nullable: false),
                    GigScopeId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Packages_GigScopes_GigScopeId",
                        column: x => x.GigScopeId,
                        principalSchema: "gmt",
                        principalTable: "GigScopes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Packages_GigScopeId",
                schema: "gmt",
                table: "Packages",
                column: "GigScopeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gigs_GigScopes_GigScopeId",
                schema: "gmt",
                table: "Gigs",
                column: "GigScopeId",
                principalSchema: "gmt",
                principalTable: "GigScopes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gigs_GigScopes_GigScopeId",
                schema: "gmt",
                table: "Gigs");

            migrationBuilder.DropTable(
                name: "Packages",
                schema: "gmt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_GigScopes",
                schema: "gmt",
                table: "GigScopes");

            migrationBuilder.RenameTable(
                name: "GigScopes",
                schema: "gmt",
                newName: "GigScope",
                newSchema: "gmt");

            migrationBuilder.AddPrimaryKey(
                name: "PK_GigScope",
                schema: "gmt",
                table: "GigScope",
                column: "Id");

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
    }
}
