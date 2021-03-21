using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Gig.Infrastructure.Persistence.Migrations
{
    public partial class MakingGigScopeNonNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GigScopeId",
                schema: "gmt",
                table: "Gigs",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "GigScopeId",
                schema: "gmt",
                table: "Gigs",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");
        }
    }
}
