using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Databases.Schools.Migrations
{
    public partial class RenameTeachersSettingsColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RestrictSessionNumber",
                table: "Group",
                newName: "AccessAllCourses");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AccessAllCourses",
                table: "Group",
                newName: "RestrictSessionNumber");
        }
    }
}
