using Microsoft.EntityFrameworkCore.Migrations;

namespace Ordering.Infrastructure.Persistence.Migrations
{
    public partial class AddingResolutionToContext : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Resolution_ResolutionId",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Resolution",
                schema: "ordering",
                table: "Resolution");

            migrationBuilder.RenameTable(
                name: "Resolution",
                schema: "ordering",
                newName: "Resolutions",
                newSchema: "ordering");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Resolutions",
                schema: "ordering",
                table: "Resolutions",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Resolutions_ResolutionId",
                schema: "ordering",
                table: "Orders",
                column: "ResolutionId",
                principalSchema: "ordering",
                principalTable: "Resolutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Resolutions_ResolutionId",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Resolutions",
                schema: "ordering",
                table: "Resolutions");

            migrationBuilder.RenameTable(
                name: "Resolutions",
                schema: "ordering",
                newName: "Resolution",
                newSchema: "ordering");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Resolution",
                schema: "ordering",
                table: "Resolution",
                column: "Id");

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
    }
}
