using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Billing.Infrastructure.Persistence.Migrations
{
    public partial class AddingConfigurations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "billing");

            migrationBuilder.CreateTable(
                name: "account_holders",
                schema: "billing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_account_holders", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "payment_methods",
                schema: "billing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Alias = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    AccountId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_payment_methods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "accounts",
                schema: "billing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AccountHolderId = table.Column<int>(type: "int", nullable: false),
                    DefaultPaymentMethodId = table.Column<int>(type: "int", nullable: true),
                    DefaultPaymentMethodId1 = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_accounts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_accounts_payment_methods_DefaultPaymentMethodId1",
                        column: x => x.DefaultPaymentMethodId1,
                        principalSchema: "billing",
                        principalTable: "payment_methods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "transactions",
                schema: "billing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TransactionType = table.Column<int>(type: "int", nullable: false),
                    TransactionId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    OrderNumber = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PaymentMethodId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_transactions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_transactions_payment_methods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalSchema: "billing",
                        principalTable: "payment_methods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_account_holders_AccountId",
                schema: "billing",
                table: "account_holders",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_account_holders_UserId",
                schema: "billing",
                table: "account_holders",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_accounts_DefaultPaymentMethodId1",
                schema: "billing",
                table: "accounts",
                column: "DefaultPaymentMethodId1");

            migrationBuilder.CreateIndex(
                name: "IX_payment_methods_AccountId",
                schema: "billing",
                table: "payment_methods",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_transactions_PaymentMethodId",
                schema: "billing",
                table: "transactions",
                column: "PaymentMethodId");

            migrationBuilder.AddForeignKey(
                name: "FK_account_holders_accounts_AccountId",
                schema: "billing",
                table: "account_holders",
                column: "AccountId",
                principalSchema: "billing",
                principalTable: "accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_payment_methods_accounts_AccountId",
                schema: "billing",
                table: "payment_methods",
                column: "AccountId",
                principalSchema: "billing",
                principalTable: "accounts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_payment_methods_accounts_AccountId",
                schema: "billing",
                table: "payment_methods");

            migrationBuilder.DropTable(
                name: "account_holders",
                schema: "billing");

            migrationBuilder.DropTable(
                name: "transactions",
                schema: "billing");

            migrationBuilder.DropTable(
                name: "accounts",
                schema: "billing");

            migrationBuilder.DropTable(
                name: "payment_methods",
                schema: "billing");
        }
    }
}
