using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Infrastructure.Persistence.Migrations
{
    public partial class settingnoactionbehaviourfororders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Gigs_GigId",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Gigs_GigId",
                table: "Orders",
                column: "GigId",
                principalTable: "Gigs",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Gigs_GigId",
                table: "Orders");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Gigs_GigId",
                table: "Orders",
                column: "GigId",
                principalTable: "Gigs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
