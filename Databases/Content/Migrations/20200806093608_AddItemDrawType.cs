using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Databases.Content.Migrations
{
    public partial class AddItemDrawType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LineColor",
                table: "Item",
                nullable: true
            );

            migrationBuilder.AddColumn<int>(
                name: "LineWidth",
                table: "Item",
                nullable: true
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LineColor",
                table: "Item"
            );

            migrationBuilder.DropColumn(
                name: "LineWidth",
                table: "Item"
            );
        }
    }
}
