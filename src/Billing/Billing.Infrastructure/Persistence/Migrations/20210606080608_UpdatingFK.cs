using Microsoft.EntityFrameworkCore.Migrations;

namespace Billing.Infrastructure.Persistence.Migrations
{
    public partial class UpdatingFK : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountHolders_Wallets_WalletId",
                schema: "billing",
                table: "AccountHolders");

            migrationBuilder.DropIndex(
                name: "IX_AccountHolders_WalletId",
                schema: "billing",
                table: "AccountHolders");

            migrationBuilder.CreateIndex(
                name: "IX_Wallets_AccountHolderId",
                schema: "billing",
                table: "Wallets",
                column: "AccountHolderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Wallets_AccountHolders_AccountHolderId",
                schema: "billing",
                table: "Wallets",
                column: "AccountHolderId",
                principalSchema: "billing",
                principalTable: "AccountHolders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Wallets_AccountHolders_AccountHolderId",
                schema: "billing",
                table: "Wallets");

            migrationBuilder.DropIndex(
                name: "IX_Wallets_AccountHolderId",
                schema: "billing",
                table: "Wallets");

            migrationBuilder.CreateIndex(
                name: "IX_AccountHolders_WalletId",
                schema: "billing",
                table: "AccountHolders",
                column: "WalletId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_AccountHolders_Wallets_WalletId",
                schema: "billing",
                table: "AccountHolders",
                column: "WalletId",
                principalSchema: "billing",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
