using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.UserProfile.Infrastructure.Persistence.Migrations
{
    public partial class UserBioAndNames : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Language_profiles_ProfileId",
                schema: "up",
                table: "Language");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_profiles_ProfileId",
                schema: "up",
                table: "Skill");

            migrationBuilder.DropTable(
                name: "NotificationSettings",
                schema: "up");

            migrationBuilder.DropPrimaryKey(
                name: "PK_profiles",
                schema: "up",
                table: "profiles");

            migrationBuilder.DropColumn(
                name: "FirstName",
                schema: "up",
                table: "profiles");

            migrationBuilder.DropColumn(
                name: "LastName",
                schema: "up",
                table: "profiles");

            migrationBuilder.RenameTable(
                name: "profiles",
                schema: "up",
                newName: "Profiles",
                newSchema: "up");

            migrationBuilder.RenameColumn(
                name: "Description",
                schema: "up",
                table: "Profiles",
                newName: "Bio");

            migrationBuilder.AddColumn<string>(
                name: "GivenName",
                schema: "up",
                table: "Profiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Surname",
                schema: "up",
                table: "Profiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Profiles",
                schema: "up",
                table: "Profiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Language_Profiles_ProfileId",
                schema: "up",
                table: "Language",
                column: "ProfileId",
                principalSchema: "up",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_Profiles_ProfileId",
                schema: "up",
                table: "Skill",
                column: "ProfileId",
                principalSchema: "up",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Language_Profiles_ProfileId",
                schema: "up",
                table: "Language");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_Profiles_ProfileId",
                schema: "up",
                table: "Skill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Profiles",
                schema: "up",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "GivenName",
                schema: "up",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "Surname",
                schema: "up",
                table: "Profiles");

            migrationBuilder.RenameTable(
                name: "Profiles",
                schema: "up",
                newName: "profiles",
                newSchema: "up");

            migrationBuilder.RenameColumn(
                name: "Bio",
                schema: "up",
                table: "profiles",
                newName: "Description");

            migrationBuilder.AddColumn<string>(
                name: "FirstName",
                schema: "up",
                table: "profiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastName",
                schema: "up",
                table: "profiles",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_profiles",
                schema: "up",
                table: "profiles",
                column: "Id");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Language_profiles_ProfileId",
                schema: "up",
                table: "Language",
                column: "ProfileId",
                principalSchema: "up",
                principalTable: "profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_profiles_ProfileId",
                schema: "up",
                table: "Skill",
                column: "ProfileId",
                principalSchema: "up",
                principalTable: "profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
