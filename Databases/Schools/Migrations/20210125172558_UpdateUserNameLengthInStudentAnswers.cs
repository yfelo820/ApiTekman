using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Databases.Schools.Migrations
{
    public partial class UpdateUserNameLengthInStudentAnswers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "StudentAnswer",
                type: "VARCHAR(450)",
                maxLength: 450,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "StudentGrade",
                table: "StudentAnswer",
                nullable: true,
                oldClrType: typeof(float));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserName",
                table: "StudentAnswer",
                type: "VARCHAR(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "VARCHAR(450)",
                oldMaxLength: 450,
                oldNullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "StudentGrade",
                table: "StudentAnswer",
                nullable: false,
                oldClrType: typeof(float),
                oldNullable: true);
        }
    }
}
