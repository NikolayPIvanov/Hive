using Microsoft.EntityFrameworkCore.Migrations;

namespace Billing.Infrastructure.Persistence.Migrations
{
    public partial class UpdatingFK4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "WalletId",
                schema: "billing",
                table: "AccountHolders");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "WalletId",
                schema: "billing",
                table: "AccountHolders",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
