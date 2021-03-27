using Microsoft.EntityFrameworkCore.Migrations;

namespace Ordering.Infrastructure.Persistence.Migrations
{
    public partial class MakingOrderResoulutionNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Resolutions_ResolutionId",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ResolutionId",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "ResolutionId",
                schema: "ordering",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ResolutionId",
                schema: "ordering",
                table: "Orders",
                column: "ResolutionId",
                unique: true,
                filter: "[ResolutionId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Resolutions_ResolutionId",
                schema: "ordering",
                table: "Orders",
                column: "ResolutionId",
                principalSchema: "ordering",
                principalTable: "Resolutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Resolutions_ResolutionId",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ResolutionId",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.AlterColumn<int>(
                name: "ResolutionId",
                schema: "ordering",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ResolutionId",
                schema: "ordering",
                table: "Orders",
                column: "ResolutionId",
                unique: true);

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
    }
}
