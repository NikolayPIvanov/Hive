using Microsoft.EntityFrameworkCore.Migrations;

namespace Ordering.Infrastructure.Persistence.Migrations
{
    public partial class OrderingQuantity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_requirements_Orders_OrderId",
                schema: "ordering",
                table: "requirements");

            migrationBuilder.DropForeignKey(
                name: "FK_Resolutions_Orders_OrderId1",
                schema: "ordering",
                table: "Resolutions");

            migrationBuilder.DropForeignKey(
                name: "FK_State_Orders_OrderId",
                schema: "ordering",
                table: "State");

            migrationBuilder.DropIndex(
                name: "IX_Resolutions_OrderId1",
                schema: "ordering",
                table: "Resolutions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_requirements",
                schema: "ordering",
                table: "requirements");

            migrationBuilder.DropIndex(
                name: "IX_Orders_OrderNumber",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropPrimaryKey(
                name: "PK_State",
                schema: "ordering",
                table: "State");

            migrationBuilder.DropColumn(
                name: "OrderId1",
                schema: "ordering",
                table: "Resolutions");

            migrationBuilder.RenameTable(
                name: "requirements",
                schema: "ordering",
                newName: "Requirements",
                newSchema: "ordering");

            migrationBuilder.RenameTable(
                name: "State",
                schema: "ordering",
                newName: "OrderStates",
                newSchema: "ordering");

            migrationBuilder.RenameIndex(
                name: "IX_requirements_OrderId",
                schema: "ordering",
                table: "Requirements",
                newName: "IX_Requirements_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_State_OrderId",
                schema: "ordering",
                table: "OrderStates",
                newName: "IX_OrderStates_OrderId");

            migrationBuilder.AlterColumn<int>(
                name: "BuyerId",
                schema: "ordering",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Quantity",
                schema: "ordering",
                table: "Orders",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                schema: "ordering",
                table: "Orders",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "ordering",
                table: "Buyers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Requirements",
                schema: "ordering",
                table: "Requirements",
                column: "Id");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Orders_OrderNumber",
                schema: "ordering",
                table: "Orders",
                column: "OrderNumber")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Buyers_UserId",
                schema: "ordering",
                table: "Buyers",
                column: "UserId")
                .Annotation("SqlServer:Clustered", false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_OrderStates",
                schema: "ordering",
                table: "OrderStates",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OrderStates_Orders_OrderId",
                schema: "ordering",
                table: "OrderStates",
                column: "OrderId",
                principalSchema: "ordering",
                principalTable: "Orders",
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OrderStates_Orders_OrderId",
                schema: "ordering",
                table: "OrderStates");

            migrationBuilder.DropForeignKey(
                name: "FK_Requirements_Orders_OrderId",
                schema: "ordering",
                table: "Requirements");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Requirements",
                schema: "ordering",
                table: "Requirements");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Orders_OrderNumber",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Buyers_UserId",
                schema: "ordering",
                table: "Buyers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_OrderStates",
                schema: "ordering",
                table: "OrderStates");

            migrationBuilder.DropColumn(
                name: "Quantity",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                schema: "ordering",
                table: "Orders");

            migrationBuilder.RenameTable(
                name: "Requirements",
                schema: "ordering",
                newName: "requirements",
                newSchema: "ordering");

            migrationBuilder.RenameTable(
                name: "OrderStates",
                schema: "ordering",
                newName: "State",
                newSchema: "ordering");

            migrationBuilder.RenameIndex(
                name: "IX_Requirements_OrderId",
                schema: "ordering",
                table: "requirements",
                newName: "IX_requirements_OrderId");

            migrationBuilder.RenameIndex(
                name: "IX_OrderStates_OrderId",
                schema: "ordering",
                table: "State",
                newName: "IX_State_OrderId");

            migrationBuilder.AddColumn<int>(
                name: "OrderId1",
                schema: "ordering",
                table: "Resolutions",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "BuyerId",
                schema: "ordering",
                table: "Orders",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                schema: "ordering",
                table: "Buyers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_requirements",
                schema: "ordering",
                table: "requirements",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_State",
                schema: "ordering",
                table: "State",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Resolutions_OrderId1",
                schema: "ordering",
                table: "Resolutions",
                column: "OrderId1");

            migrationBuilder.CreateIndex(
                name: "IX_Orders_OrderNumber",
                schema: "ordering",
                table: "Orders",
                column: "OrderNumber",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_requirements_Orders_OrderId",
                schema: "ordering",
                table: "requirements",
                column: "OrderId",
                principalSchema: "ordering",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Resolutions_Orders_OrderId1",
                schema: "ordering",
                table: "Resolutions",
                column: "OrderId1",
                principalSchema: "ordering",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_State_Orders_OrderId",
                schema: "ordering",
                table: "State",
                column: "OrderId",
                principalSchema: "ordering",
                principalTable: "Orders",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
