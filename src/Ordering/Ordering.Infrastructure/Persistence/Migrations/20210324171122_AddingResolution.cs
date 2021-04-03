using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ordering.Infrastructure.Persistence.Migrations
{
    public partial class AddingResolution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ResolutionId",
                schema: "ordering",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Resolution",
                schema: "ordering",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Version = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Location = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Resolution", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ResolutionId",
                schema: "ordering",
                table: "Orders",
                column: "ResolutionId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Resolution_ResolutionId",
                schema: "ordering",
                table: "Orders",
                column: "ResolutionId",
                principalSchema: "ordering",
                principalTable: "Resolution",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Resolution_ResolutionId",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Resolution",
                schema: "ordering");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ResolutionId",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ResolutionId",
                schema: "ordering",
                table: "Orders");
        }
    }
}
