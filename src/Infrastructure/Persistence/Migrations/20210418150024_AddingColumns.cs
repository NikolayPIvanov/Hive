using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Infrastructure.Persistence.Migrations
{
    public partial class AddingColumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_requirements_Orders_OrderId",
                table: "requirements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_requirements",
                table: "requirements");

            migrationBuilder.RenameTable(
                name: "requirements",
                newName: "Requirements");

            migrationBuilder.RenameIndex(
                name: "IX_requirements_OrderId",
                table: "Requirements",
                newName: "IX_Requirements_OrderId");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAccepted",
                table: "Investments",
                type: "bit",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "bit",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "InvestorId1",
                table: "Investments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlanId",
                table: "Gigs",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Requirements",
                table: "Requirements",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Investments_InvestorId1",
                table: "Investments",
                column: "InvestorId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Investments_Investors_InvestorId1",
                table: "Investments",
                column: "InvestorId1",
                principalTable: "Investors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Requirements_Orders_OrderId",
                table: "Requirements",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investments_Investors_InvestorId1",
                table: "Investments");

            migrationBuilder.DropForeignKey(
                name: "FK_Requirements_Orders_OrderId",
                table: "Requirements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Requirements",
                table: "Requirements");

            migrationBuilder.DropIndex(
                name: "IX_Investments_InvestorId1",
                table: "Investments");

            migrationBuilder.DropColumn(
                name: "InvestorId1",
                table: "Investments");

            migrationBuilder.DropColumn(
                name: "PlanId",
                table: "Gigs");

            migrationBuilder.RenameTable(
                name: "Requirements",
                newName: "requirements");

            migrationBuilder.RenameIndex(
                name: "IX_Requirements_OrderId",
                table: "requirements",
                newName: "IX_requirements_OrderId");

            migrationBuilder.AlterColumn<bool>(
                name: "IsAccepted",
                table: "Investments",
                type: "bit",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "bit");

            migrationBuilder.AddPrimaryKey(
                name: "PK_requirements",
                table: "requirements",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_requirements_Orders_OrderId",
                table: "requirements",
                column: "OrderId",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
