using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Investing.Infrastructure.Persistence.Migrations
{
    public partial class UpdatingPlans : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EstimatedReleaseDate",
                schema: "investing",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "EstimatedReleaseDays",
                schema: "investing",
                table: "Plans");

            migrationBuilder.RenameColumn(
                name: "FundingNeeded",
                schema: "investing",
                table: "Plans",
                newName: "StartingFunds");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "investing",
                table: "Plans",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "investing",
                table: "Plans",
                type: "nvarchar(3000)",
                maxLength: 3000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldMaxLength: 5000);

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                schema: "investing",
                table: "Plans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "IsPublic",
                schema: "investing",
                table: "Plans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                schema: "investing",
                table: "Plans",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EndDate",
                schema: "investing",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "IsPublic",
                schema: "investing",
                table: "Plans");

            migrationBuilder.DropColumn(
                name: "StartDate",
                schema: "investing",
                table: "Plans");

            migrationBuilder.RenameColumn(
                name: "StartingFunds",
                schema: "investing",
                table: "Plans",
                newName: "FundingNeeded");

            migrationBuilder.AlterColumn<string>(
                name: "Title",
                schema: "investing",
                table: "Plans",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(50)",
                oldMaxLength: 50);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                schema: "investing",
                table: "Plans",
                type: "nvarchar(max)",
                maxLength: 5000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(3000)",
                oldMaxLength: 3000);

            migrationBuilder.AddColumn<DateTime>(
                name: "EstimatedReleaseDate",
                schema: "investing",
                table: "Plans",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EstimatedReleaseDays",
                schema: "investing",
                table: "Plans",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
