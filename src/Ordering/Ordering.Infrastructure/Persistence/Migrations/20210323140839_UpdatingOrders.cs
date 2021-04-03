using Microsoft.EntityFrameworkCore.Migrations;

namespace Ordering.Infrastructure.Persistence.Migrations
{
    public partial class UpdatingOrders : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UnitAmount",
                schema: "ordering",
                table: "Orders",
                newName: "UnitPrice");

            migrationBuilder.AddColumn<bool>(
                name: "IsClosed",
                schema: "ordering",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsClosed",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.RenameColumn(
                name: "UnitPrice",
                schema: "ordering",
                table: "Orders",
                newName: "UnitAmount");
        }
    }
}
