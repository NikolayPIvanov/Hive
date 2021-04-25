using Microsoft.EntityFrameworkCore.Migrations;

namespace Billing.Infrastructure.Persistence.Migrations
{
    public partial class UpdatingBillingDomain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountHolders_Wallets_WalletId1",
                schema: "billing",
                table: "AccountHolders");

            migrationBuilder.DropIndex(
                name: "IX_AccountHolders_WalletId1",
                schema: "billing",
                table: "AccountHolders");

            migrationBuilder.DropColumn(
                name: "WalletId1",
                schema: "billing",
                table: "AccountHolders");

            migrationBuilder.RenameColumn(
                name: "PublicId",
                schema: "billing",
                table: "Transactions",
                newName: "TransactionNumber");

            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                schema: "billing",
                table: "Wallets",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "TransactionType",
                schema: "billing",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "billing",
                table: "AccountHolders",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Transactions_TransactionNumber",
                schema: "billing",
                table: "Transactions",
                column: "TransactionNumber")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_AccountHolders_UserId",
                schema: "billing",
                table: "AccountHolders",
                column: "UserId")
                .Annotation("SqlServer:Clustered", false);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccountHolders_Wallets_WalletId",
                schema: "billing",
                table: "AccountHolders");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Transactions_TransactionNumber",
                schema: "billing",
                table: "Transactions");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_AccountHolders_UserId",
                schema: "billing",
                table: "AccountHolders");

            migrationBuilder.DropIndex(
                name: "IX_AccountHolders_WalletId",
                schema: "billing",
                table: "AccountHolders");

            migrationBuilder.DropColumn(
                name: "Balance",
                schema: "billing",
                table: "Wallets");

            migrationBuilder.RenameColumn(
                name: "TransactionNumber",
                schema: "billing",
                table: "Transactions",
                newName: "PublicId");

            migrationBuilder.AlterColumn<int>(
                name: "TransactionType",
                schema: "billing",
                table: "Transactions",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "billing",
                table: "AccountHolders",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "WalletId1",
                schema: "billing",
                table: "AccountHolders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AccountHolders_WalletId1",
                schema: "billing",
                table: "AccountHolders",
                column: "WalletId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AccountHolders_Wallets_WalletId1",
                schema: "billing",
                table: "AccountHolders",
                column: "WalletId1",
                principalSchema: "billing",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
