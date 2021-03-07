using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Infrastructure.Persistence.Migrations
{
    public partial class AddingIsSellerpropertytouser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsSeller",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsSeller",
                table: "AspNetUsers");
        }
    }
}
