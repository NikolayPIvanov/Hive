using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Ordering.Infrastructure.Persistence.Migrations
{
    public partial class AddingOrdersAgain : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Order_Requirement_RequirementId",
                schema: "ordering",
                table: "Order");

            migrationBuilder.DropForeignKey(
                name: "FK_Requirements_Orders_OrderId",
                schema: "ordering",
                table: "Requirements");

            migrationBuilder.DropIndex(
                name: "IX_Requirements_OrderId",
                schema: "ordering",
                table: "Requirements");

            migrationBuilder.DropIndex(
                name: "IX_Order_OrderNumber",
                schema: "ordering",
                table: "Order");

            migrationBuilder.DropIndex(
                name: "IX_Order_RequirementId",
                schema: "ordering",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Created",
                schema: "ordering",
                table: "Requirements");

            migrationBuilder.DropColumn(
                name: "AcceptedAt",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "CanceledBy",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Created",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "DeclinedAt",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OfferedById",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "OrderedById",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.EnsureSchema(
                name: "orderng");

            migrationBuilder.RenameTable(
                name: "Requirements",
                schema: "ordering",
                newName: "Requirements",
                newSchema: "orderng");

            migrationBuilder.RenameTable(
                name: "Requirement",
                schema: "ordering",
                newName: "Requirement",
                newSchema: "orderng");

            migrationBuilder.RenameTable(
                name: "Orders",
                schema: "ordering",
                newName: "Orders",
                newSchema: "orderng");

            migrationBuilder.RenameTable(
                name: "Order",
                schema: "ordering",
                newName: "Order",
                newSchema: "orderng");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                schema: "orderng",
                table: "Orders",
                newName: "UnitAmount");

            migrationBuilder.RenameColumn(
                name: "UnitAmount",
                schema: "orderng",
                table: "Order",
                newName: "TotalAmount");

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                schema: "orderng",
                table: "Requirements",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2500)",
                oldMaxLength: 2500);

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                schema: "orderng",
                table: "Requirement",
                type: "nvarchar(2500)",
                maxLength: 2500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                schema: "orderng",
                table: "Requirement",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "AcceptedAt",
                schema: "orderng",
                table: "Order",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CanceledBy",
                schema: "orderng",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                schema: "orderng",
                table: "Order",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeclinedAt",
                schema: "orderng",
                table: "Order",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                schema: "orderng",
                table: "Order",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OfferedById",
                schema: "orderng",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrderedById",
                schema: "orderng",
                table: "Order",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "orderng",
                table: "Order",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Requirement_OrderId",
                schema: "orderng",
                table: "Requirement",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber",
                schema: "orderng",
                table: "Orders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Orders_RequirementId",
                schema: "orderng",
                table: "Orders",
                column: "RequirementId",
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

            migrationBuilder.AddForeignKey(
                name: "FK_Requirement_Order_OrderId",
                schema: "orderng",
                table: "Requirement",
                column: "OrderId",
                principalSchema: "orderng",
                principalTable: "Order",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Orders_Requirements_RequirementId",
                schema: "orderng",
                table: "Orders");

            migrationBuilder.DropForeignKey(
                name: "FK_Requirement_Order_OrderId",
                schema: "orderng",
                table: "Requirement");

            migrationBuilder.DropIndex(
                name: "IX_Requirement_OrderId",
                schema: "orderng",
                table: "Requirement");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderNumber",
                schema: "orderng",
                table: "Orders");

            migrationBuilder.DropIndex(
                name: "IX_Orders_RequirementId",
                schema: "orderng",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "Created",
                schema: "orderng",
                table: "Requirement");

            migrationBuilder.DropColumn(
                name: "AcceptedAt",
                schema: "orderng",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "CanceledBy",
                schema: "orderng",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Created",
                schema: "orderng",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "DeclinedAt",
                schema: "orderng",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "IsCanceled",
                schema: "orderng",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OfferedById",
                schema: "orderng",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "OrderedById",
                schema: "orderng",
                table: "Order");

            migrationBuilder.DropColumn(
                name: "Status",
                schema: "orderng",
                table: "Order");

            migrationBuilder.EnsureSchema(
                name: "ordering");

            migrationBuilder.RenameTable(
                name: "Requirements",
                schema: "orderng",
                newName: "Requirements",
                newSchema: "ordering");

            migrationBuilder.RenameTable(
                name: "Requirement",
                schema: "orderng",
                newName: "Requirement",
                newSchema: "ordering");

            migrationBuilder.RenameTable(
                name: "Orders",
                schema: "orderng",
                newName: "Orders",
                newSchema: "ordering");

            migrationBuilder.RenameTable(
                name: "Order",
                schema: "orderng",
                newName: "Order",
                newSchema: "ordering");

            migrationBuilder.RenameColumn(
                name: "UnitAmount",
                schema: "ordering",
                table: "Orders",
                newName: "TotalAmount");

            migrationBuilder.RenameColumn(
                name: "TotalAmount",
                schema: "ordering",
                table: "Order",
                newName: "UnitAmount");

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

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                schema: "ordering",
                table: "Requirements",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AlterColumn<string>(
                name: "Details",
                schema: "ordering",
                table: "Requirement",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(2500)",
                oldMaxLength: 2500);

            migrationBuilder.AddColumn<DateTime>(
                name: "AcceptedAt",
                schema: "ordering",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CanceledBy",
                schema: "ordering",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Created",
                schema: "ordering",
                table: "Orders",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeclinedAt",
                schema: "ordering",
                table: "Orders",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsCanceled",
                schema: "ordering",
                table: "Orders",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "OfferedById",
                schema: "ordering",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "OrderedById",
                schema: "ordering",
                table: "Orders",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Status",
                schema: "ordering",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Requirements_OrderId",
                schema: "ordering",
                table: "Requirements",
                column: "OrderId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_OrderNumber",
                schema: "ordering",
                table: "Order",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Order_RequirementId",
                schema: "ordering",
                table: "Order",
                column: "RequirementId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Order_Requirement_RequirementId",
                schema: "ordering",
                table: "Order",
                column: "RequirementId",
                principalSchema: "ordering",
                principalTable: "Requirement",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
