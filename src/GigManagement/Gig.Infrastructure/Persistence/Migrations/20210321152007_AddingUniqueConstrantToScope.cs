using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Gig.Infrastructure.Persistence.Migrations
{
    public partial class AddingUniqueConstrantToScope : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_GigScopes_GigId",
                schema: "gmt",
                table: "GigScopes",
                column: "GigId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_GigScopes_GigId",
                schema: "gmt",
                table: "GigScopes");
        }
    }
}
