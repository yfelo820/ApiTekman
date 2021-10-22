using Api.Constants;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Databases.Content.Migrations
{
    public partial class AddSubjectKey : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Key",
                table: "Subject",
                nullable: true);

			migrationBuilder.Sql(
				"UPDATE [Subject] SET [Key]='" + SubjectKey.Emat + "' WHERE [name] like '%EMAT%'"
			);
			migrationBuilder.Sql(
				"UPDATE [Subject] SET [Key]='" + SubjectKey.Ludi + "' WHERE [name] like '%LUDI%'"
			);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Key",
                table: "Subject");
        }
    }
}
