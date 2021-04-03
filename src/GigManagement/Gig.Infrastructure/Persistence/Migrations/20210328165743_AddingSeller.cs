using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Gig.Infrastructure.Persistence.Migrations
{
    public partial class AddingSeller : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SellerId",
                schema: "gmt",
                table: "Gigs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Sellers",
                schema: "gmt",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sellers", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Gigs_SellerId",
                schema: "gmt",
                table: "Gigs",
                column: "SellerId");

            migrationBuilder.AddForeignKey(
                name: "FK_Gigs_Sellers_SellerId",
                schema: "gmt",
                table: "Gigs",
                column: "SellerId",
                principalSchema: "gmt",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Gigs_Sellers_SellerId",
                schema: "gmt",
                table: "Gigs");

            migrationBuilder.DropTable(
                name: "Sellers",
                schema: "gmt");

            migrationBuilder.DropIndex(
                name: "IX_Gigs_SellerId",
                schema: "gmt",
                table: "Gigs");

            migrationBuilder.DropColumn(
                name: "SellerId",
                schema: "gmt",
                table: "Gigs");
        }
    }
}
