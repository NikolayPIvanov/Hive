using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Investing.Infrastructure.Persistence.Migrations
{
    public partial class AddingGigId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GigId",
                schema: "investing",
                table: "Plans",
                type: "int",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "GigId",
                schema: "investing",
                table: "Plans");
        }
    }
}
