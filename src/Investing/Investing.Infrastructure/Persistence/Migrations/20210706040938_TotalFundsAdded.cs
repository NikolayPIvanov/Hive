using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Investing.Infrastructure.Persistence.Migrations
{
    public partial class TotalFundsAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Investments_Investors_InvestorId1",
                schema: "investing",
                table: "Investments");

            migrationBuilder.DropTable(
                name: "SearchTag",
                schema: "investing");

            migrationBuilder.DropIndex(
                name: "IX_Investments_InvestorId1",
                schema: "investing",
                table: "Investments");

            migrationBuilder.DropColumn(
                name: "InvestorId1",
                schema: "investing",
                table: "Investments");

            migrationBuilder.RenameColumn(
                name: "StartingFunds",
                schema: "investing",
                table: "Plans",
                newName: "TotalFundsNeeded");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalFundsNeeded",
                schema: "investing",
                table: "Plans",
                newName: "StartingFunds");

            migrationBuilder.AddColumn<int>(
                name: "InvestorId1",
                schema: "investing",
                table: "Investments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SearchTag",
                schema: "investing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SearchTag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SearchTag_Plans_PlanId",
                        column: x => x.PlanId,
                        principalSchema: "investing",
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Investments_InvestorId1",
                schema: "investing",
                table: "Investments",
                column: "InvestorId1");

            migrationBuilder.CreateIndex(
                name: "IX_SearchTag_PlanId",
                schema: "investing",
                table: "SearchTag",
                column: "PlanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Investments_Investors_InvestorId1",
                schema: "investing",
                table: "Investments",
                column: "InvestorId1",
                principalSchema: "investing",
                principalTable: "Investors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
