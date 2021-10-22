using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Databases.Schools.Migrations
{
    public partial class StudentAnswerStudentGrade : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "StudentGrade",
                table: "StudentAnswer",
                nullable: true);
            migrationBuilder.Sql(@"
                UPDATE StudentAnswer SET StudentGrade = Grade
                WHERE SubjectKey = 'ematinfantil'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StudentGrade",
                table: "StudentAnswer");
        }
    }
}
