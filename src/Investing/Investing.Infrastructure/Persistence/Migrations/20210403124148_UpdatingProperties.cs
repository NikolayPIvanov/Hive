using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Investing.Infrastructure.Persistence.Migrations
{
    public partial class UpdatingProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "investing");

            migrationBuilder.CreateTable(
                name: "investors",
                schema: "investing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_investors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "vendors",
                schema: "investing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_vendors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "plans",
                schema: "investing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    EstimatedReleaseDays = table.Column<int>(type: "int", nullable: false),
                    EstimatedReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FundingNeeded = table.Column<decimal>(type: "decimal(18,2)", precision: 18, scale: 2, nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    InvestmentId = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_plans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_plans_vendors_VendorId",
                        column: x => x.VendorId,
                        principalSchema: "investing",
                        principalTable: "vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "investments",
                schema: "investing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EffectiveDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    ExpirationDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RoiPercentage = table.Column<double>(type: "float", nullable: false),
                    InvestorId = table.Column<int>(type: "int", nullable: false),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    PlanId2 = table.Column<int>(type: "int", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_investments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_investments_investors_InvestorId",
                        column: x => x.InvestorId,
                        principalSchema: "investing",
                        principalTable: "investors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_investments_plans_PlanId2",
                        column: x => x.PlanId2,
                        principalSchema: "investing",
                        principalTable: "plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Tag",
                schema: "investing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PlanId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tag", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tag_plans_PlanId",
                        column: x => x.PlanId,
                        principalSchema: "investing",
                        principalTable: "plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_investments_InvestorId",
                schema: "investing",
                table: "investments",
                column: "InvestorId");

            migrationBuilder.CreateIndex(
                name: "IX_investments_PlanId2",
                schema: "investing",
                table: "investments",
                column: "PlanId2");

            migrationBuilder.CreateIndex(
                name: "IX_investors_UserId",
                schema: "investing",
                table: "investors",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_plans_InvestmentId",
                schema: "investing",
                table: "plans",
                column: "InvestmentId",
                unique: true,
                filter: "[InvestmentId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_plans_VendorId",
                schema: "investing",
                table: "plans",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_PlanId",
                schema: "investing",
                table: "Tag",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_vendors_UserId",
                schema: "investing",
                table: "vendors",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_plans_investments_InvestmentId",
                schema: "investing",
                table: "plans",
                column: "InvestmentId",
                principalSchema: "investing",
                principalTable: "investments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_investments_investors_InvestorId",
                schema: "investing",
                table: "investments");

            migrationBuilder.DropForeignKey(
                name: "FK_investments_plans_PlanId2",
                schema: "investing",
                table: "investments");

            migrationBuilder.DropTable(
                name: "Tag",
                schema: "investing");

            migrationBuilder.DropTable(
                name: "investors",
                schema: "investing");

            migrationBuilder.DropTable(
                name: "plans",
                schema: "investing");

            migrationBuilder.DropTable(
                name: "investments",
                schema: "investing");

            migrationBuilder.DropTable(
                name: "vendors",
                schema: "investing");
        }
    }
}
