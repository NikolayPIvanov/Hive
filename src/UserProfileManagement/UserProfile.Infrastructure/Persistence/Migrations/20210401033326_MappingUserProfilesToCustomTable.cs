using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.UserProfile.Infrastructure.Persistence.Migrations
{
    public partial class MappingUserProfilesToCustomTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Language_UserProfiles_ProfileId",
                schema: "up",
                table: "Language");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_UserProfiles_ProfileId",
                schema: "up",
                table: "Skill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserProfiles",
                schema: "up",
                table: "UserProfiles");

            migrationBuilder.RenameTable(
                name: "UserProfiles",
                schema: "up",
                newName: "profiles",
                newSchema: "up");

            migrationBuilder.AddPrimaryKey(
                name: "PK_profiles",
                schema: "up",
                table: "profiles",
                column: "Id");

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Language_profiles_ProfileId",
                schema: "up",
                table: "Language");

            migrationBuilder.DropForeignKey(
                name: "FK_Skill_profiles_ProfileId",
                schema: "up",
                table: "Skill");

            migrationBuilder.DropPrimaryKey(
                name: "PK_profiles",
                schema: "up",
                table: "profiles");

            migrationBuilder.RenameTable(
                name: "profiles",
                schema: "up",
                newName: "UserProfiles",
                newSchema: "up");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProfiles",
                schema: "up",
                table: "UserProfiles",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Language_UserProfiles_ProfileId",
                schema: "up",
                table: "Language",
                column: "ProfileId",
                principalSchema: "up",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Skill_UserProfiles_ProfileId",
                schema: "up",
                table: "Skill",
                column: "ProfileId",
                principalSchema: "up",
                principalTable: "UserProfiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
