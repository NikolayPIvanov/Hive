using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.UserProfile.Infrastructure.Persistence.Migrations
{
    public partial class AddingEntitiesInProfilesManagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationSettings",
                schema: "up",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmailNotifications = table.Column<bool>(type: "bit", nullable: false),
                    ProfileId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationSettings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationSettings_profiles_ProfileId",
                        column: x => x.ProfileId,
                        principalSchema: "up",
                        principalTable: "profiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationSettings_ProfileId",
                schema: "up",
                table: "NotificationSettings",
                column: "ProfileId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationSettings",
                schema: "up");
        }
    }
}
