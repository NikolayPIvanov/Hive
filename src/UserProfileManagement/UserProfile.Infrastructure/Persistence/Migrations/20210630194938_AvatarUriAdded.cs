using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.UserProfile.Infrastructure.Persistence.Migrations
{
    public partial class AvatarUriAdded : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvatarFile",
                schema: "up",
                table: "Profiles",
                newName: "AvatarUri");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AvatarUri",
                schema: "up",
                table: "Profiles",
                newName: "AvatarFile");
        }
    }
}
