using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Databases.Schools.Migrations
{
    public partial class AddAccessNumberUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "Index_StudentGroup_UniqueAccessNumber",
                table: "StudentGroup",
                columns: new[] { "GroupId", "AccessNumber" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_StudentGroup_UniqueAccessNumber",
                table: "StudentGroup");
        }
    }
}
