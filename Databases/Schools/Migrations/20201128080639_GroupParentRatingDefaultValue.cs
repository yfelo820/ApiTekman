using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Databases.Schools.Migrations
{
    public partial class GroupParentRatingDefaultValue : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ParentRating",
                table: "Group",
                defaultValue: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<bool>(
                name: "ParentRating",
                table: "Group",
                oldDefaultValue: false);
        }
    }
}
