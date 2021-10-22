using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Databases.Schools.Migrations
{
    public partial class AddColumnsToPendingFeedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRead",
                table: "PendingFeedback",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRead",
                table: "PendingFeedback");
        }
    }
}
