using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Investing.Infrastructure.Persistence.Migrations
{
    public partial class AddingEntitiesInInvesting : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "investing");

            migrationBuilder.CreateTable(
                name: "Investors",
                schema: "investing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investors", x => x.Id);
                    table.UniqueConstraint("AK_Investors_UserId", x => x.UserId)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "Vendors",
                schema: "investing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vendors", x => x.Id);
                    table.UniqueConstraint("AK_Vendors_UserId", x => x.UserId)
                        .Annotation("SqlServer:Clustered", false);
                });

            migrationBuilder.CreateTable(
                name: "Plans",
                schema: "investing",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Title = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", maxLength: 5000, nullable: false),
                    EstimatedReleaseDays = table.Column<int>(type: "int", nullable: false),
                    EstimatedReleaseDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FundingNeeded = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsFunded = table.Column<bool>(type: "bit", nullable: false),
                    VendorId = table.Column<int>(type: "int", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Plans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Plans_Vendors_VendorId",
                        column: x => x.VendorId,
                        principalSchema: "investing",
                        principalTable: "Vendors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Investments",
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
                    InvestorId1 = table.Column<int>(type: "int", nullable: true),
                    PlanId = table.Column<int>(type: "int", nullable: false),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Investments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Investments_Investors_InvestorId",
                        column: x => x.InvestorId,
                        principalSchema: "investing",
                        principalTable: "Investors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Investments_Investors_InvestorId1",
                        column: x => x.InvestorId1,
                        principalSchema: "investing",
                        principalTable: "Investors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Investments_Plans_PlanId",
                        column: x => x.PlanId,
                        principalSchema: "investing",
                        principalTable: "Plans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SearchTag",
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
                name: "IX_Investments_InvestorId",
                schema: "investing",
                table: "Investments",
                column: "InvestorId");

            migrationBuilder.CreateIndex(
                name: "IX_Investments_InvestorId1",
                schema: "investing",
                table: "Investments",
                column: "InvestorId1");

            migrationBuilder.CreateIndex(
                name: "IX_Investments_PlanId",
                schema: "investing",
                table: "Investments",
                column: "PlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Plans_VendorId",
                schema: "investing",
                table: "Plans",
                column: "VendorId");

            migrationBuilder.CreateIndex(
                name: "IX_SearchTag_PlanId",
                schema: "investing",
                table: "SearchTag",
                column: "PlanId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Investments",
                schema: "investing");

            migrationBuilder.DropTable(
                name: "SearchTag",
                schema: "investing");

            migrationBuilder.DropTable(
                name: "Investors",
                schema: "investing");

            migrationBuilder.DropTable(
                name: "Plans",
                schema: "investing");

            migrationBuilder.DropTable(
                name: "Vendors",
                schema: "investing");
        }
    }
}
