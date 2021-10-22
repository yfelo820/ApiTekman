using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Databases.Content.Migrations
{
    public partial class RenameImageItemToMediaUrl : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Item",
                newName: "MediaUrl");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MediaUrl",
                table: "Item",
                newName: "Image");
        }
    }
}
