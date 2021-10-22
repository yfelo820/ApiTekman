using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Databases.Content.Migrations
{
    public partial class CreateAudioPlayerTypeProperty : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "LineWidth",
                table: "Item",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<byte>(
                name: "AudioPlayerType",
                table: "Item",
                nullable: false,
                defaultValue: (byte)0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AudioPlayerType",
                table: "Item");

            migrationBuilder.AlterColumn<string>(
                name: "LineWidth",
                table: "Item",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
