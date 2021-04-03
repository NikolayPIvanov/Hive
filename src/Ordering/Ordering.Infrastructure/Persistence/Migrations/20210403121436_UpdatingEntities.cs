using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ordering.Infrastructure.Persistence.Migrations
{
    public partial class UpdatingEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Resolutions_ResolutionId",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Requirements_Orders_OrderId",
                schema: "ordering",
                table: "Requirements");

            migrationBuilder.DropTable(
                name: "OrderStates",
                schema: "ordering");

            migrationBuilder.DropIndex(
                name: "IX_Requirements_OrderId",
                schema: "ordering",
                table: "Requirements");

            migrationBuilder.DropIndex(
                name: "IX_Orders_ResolutionId",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderedBy",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "ResolutionId",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "Version",
                schema: "ordering",
                table: "Resolutions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                schema: "ordering",
                table: "Resolutions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsApproved",
                schema: "ordering",
                table: "Resolutions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "BuyerId",
                schema: "ordering",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Buyers",
                schema: "ordering",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Buyers", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "State",
                schema: "ordering",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OrderState = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_State", x => x.Id);
                    table.ForeignKey(
                        name: "FK_State_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "ordering",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_OrderId",
                schema: "ordering",
                table: "Resolutions",
                column: "OrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_BuyerId",
                schema: "ordering",
                table: "Orders",
                column: "BuyerId");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RequirementId",
                schema: "ordering",
                table: "Orders",
                column: "RequirementId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_State_OrderId",
                schema: "ordering",
                table: "State",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Buyers_BuyerId",
                schema: "ordering",
                table: "Orders",
                column: "BuyerId",
                principalSchema: "ordering",
                principalTable: "Buyers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Requirements_RequirementId",
                schema: "ordering",
                table: "Orders",
                column: "RequirementId",
                principalSchema: "ordering",
                principalTable: "Requirements",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resolutions_Orders_OrderId",
                schema: "ordering",
                table: "Resolutions",
                column: "OrderId",
                principalSchema: "ordering",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Buyers_BuyerId",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Requirements_RequirementId",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Resolutions_Orders_OrderId",
                schema: "ordering",
                table: "Resolutions");

            migrationBuilder.DropTable(
                name: "Buyers",
                schema: "ordering");

            migrationBuilder.DropTable(
                name: "State",
                schema: "ordering");

            migrationBuilder.DropIndex(
                name: "IX_Resolutions_OrderId",
                schema: "ordering",
                table: "Resolutions");

            migrationBuilder.DropIndex(
                name: "IX_Orders_BuyerId",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RequirementId",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsApproved",
                schema: "ordering",
                table: "Resolutions");

            migrationBuilder.DropColumn(
                name: "BuyerId",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.AlterColumn<string>(
                name: "Version",
                schema: "ordering",
                table: "Resolutions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                schema: "ordering",
                table: "Resolutions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "OrderedBy",
                schema: "ordering",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ResolutionId",
                schema: "ordering",
                table: "Orders",
                type: "int",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "OrderStates",
                schema: "ordering",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OrderId = table.Column<int>(type: "int", nullable: false),
                    OrderState = table.Column<int>(type: "int", nullable: false),
                    Reason = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OrderStates", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OrderStates_Orders_OrderId",
                        column: x => x.OrderId,
                        principalSchema: "ordering",
                        principalTable: "Orders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Requirements_OrderId",
                schema: "ordering",
                table: "Requirements",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_ResolutionId",
                schema: "ordering",
                table: "Orders",
                column: "ResolutionId",
                unique: true,
                filter: "[ResolutionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_OrderStates_OrderId",
                schema: "ordering",
                table: "OrderStates",
                column: "OrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_Resolutions_ResolutionId",
                schema: "ordering",
                table: "Orders",
                column: "ResolutionId",
                principalSchema: "ordering",
                principalTable: "Resolutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
    }
}
