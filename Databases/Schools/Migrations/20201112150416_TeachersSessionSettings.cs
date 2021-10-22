using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Databases.Schools.Migrations
{
    public partial class TeachersSessionSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AccessFromHome",
                table: "Group",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ParentRating",
                table: "Group",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "RestrictSessionNumber",
                table: "Group",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AccessFromHome",
                table: "Group");

            migrationBuilder.DropColumn(
                name: "ParentRating",
                table: "Group");

            migrationBuilder.DropColumn(
                name: "RestrictSessionNumber",
                table: "Group");
        }
    }
}
