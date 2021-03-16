using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Hive.Infrastructure.Persistence.Migrations
{
    public partial class addingreviews : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Profiles_Sellers_SellerId",
                table: "Profiles");

            migrationBuilder.DropIndex(
                name: "IX_Profiles_SellerId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "SellerId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "IsSeller",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Profiles",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AccountType",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserProfileId",
                table: "AspNetUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Review",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GigId = table.Column<int>(type: "int", nullable: false),
                    Rating = table.Column<double>(type: "float", nullable: false),
                    Created = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LastModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifiedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Review", x => x.Id);
                    table.CheckConstraint("CK_Review_Rating", "[Rating] BETWEEN 1.0 AND 5.0");
                    table.ForeignKey(
                        name: "FK_Review_Gigs_GigId",
                        column: x => x.GigId,
                        principalTable: "Gigs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Sellers_UserProfileId",
                table: "Sellers",
                column: "UserProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_UserProfileId",
                table: "AspNetUsers",
                column: "UserProfileId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Review_GigId",
                table: "Review",
                column: "GigId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Profiles_UserProfileId",
                table: "AspNetUsers",
                column: "UserProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Sellers_Profiles_UserProfileId",
                table: "Sellers",
                column: "UserProfileId",
                principalTable: "Profiles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Profiles_UserProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropForeignKey(
                name: "FK_Sellers_Profiles_UserProfileId",
                table: "Sellers");

            migrationBuilder.DropTable(
                name: "Review");

            migrationBuilder.DropIndex(
                name: "IX_Sellers_UserProfileId",
                table: "Sellers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_UserProfileId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Profiles");

            migrationBuilder.DropColumn(
                name: "AccountType",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "UserProfileId",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "SellerId",
                table: "Profiles",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsSeller",
                table: "AspNetUsers",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Profiles_SellerId",
                table: "Profiles",
                column: "SellerId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Profiles_Sellers_SellerId",
                table: "Profiles",
                column: "SellerId",
                principalTable: "Sellers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
