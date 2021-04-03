using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ordering.Infrastructure.Persistence.Migrations
{
    public partial class AddingOrdersAndRemovingOld : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Requirements_RequirementId",
                schema: "orderng",
                table: "Orders");

            migrationBuilder.DropTable(
                name: "Requirement",
                schema: "orderng");

            migrationBuilder.DropTable(
                name: "Order",
                schema: "orderng");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RequirementId",
                schema: "orderng",
                table: "Orders");

            migrationBuilder.EnsureSchema(
                name: "ordering");

            migrationBuilder.RenameTable(
                name: "Requirements",
                schema: "orderng",
                newName: "Requirements",
                newSchema: "ordering");

            migrationBuilder.RenameTable(
                name: "Orders",
                schema: "orderng",
                newName: "Orders",
                newSchema: "ordering");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                schema: "ordering",
                table: "Requirements",
                type: "nvarchar(2500)",
                maxLength: 2500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requirements_OrderId",
                schema: "ordering",
                table: "Requirements",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Requirements_Orders_OrderId",
                schema: "ordering",
                table: "Requirements",
                column: "OrderId",
                principalSchema: "ordering",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Requirements_Orders_OrderId",
                schema: "ordering",
                table: "Requirements");

            migrationBuilder.DropIndex(
                name: "IX_Requirements_OrderId",
                schema: "ordering",
                table: "Requirements");

            migrationBuilder.EnsureSchema(
                name: "orderng");

            migrationBuilder.RenameTable(
                name: "Requirements",
                schema: "ordering",
                newName: "Requirements",
                newSchema: "orderng");

            migrationBuilder.RenameTable(
                name: "Orders",
                schema: "ordering",
                newName: "Orders",
                newSchema: "orderng");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                schema: "orderng",
                table: "Requirements",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2500)",
                oldMaxLength: 2500);

            migrationBuilder.CreateTable(
                name: "Order",
                schema: "orderng",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AcceptedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CanceledBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DeclinedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GigId = table.Column<int>(type: "int", nullable: false),
                    IsCanceled = table.Column<bool>(type: "bit", nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OfferedById = table.Column<int>(type: "int", nullable: false),
                    OrderNumber = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrderedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    OrderedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackageId = table.Column<int>(type: "int", nullable: false),
                    RequirementId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    TotalAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Order", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Requirement",
                schema: "orderng",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Details = table.Column<string>(type: "nvarchar(2500)", maxLength: 2500, nullable: false),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Requirement", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Requirement_Order_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "orderng",
                        principalTable: "Order",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RequirementId",
                schema: "orderng",
                table: "Orders",
                column: "RequirementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Requirement_OrderId",
                schema: "orderng",
                table: "Requirement",
                column: "OrderId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Requirements_RequirementId",
                schema: "orderng",
                table: "Orders",
                column: "RequirementId",
                principalSchema: "orderng",
                principalTable: "Requirements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
