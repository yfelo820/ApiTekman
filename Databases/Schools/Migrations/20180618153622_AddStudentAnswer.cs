using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Schools.Migrations
{
    public partial class AddStudentAnswer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentAnswer",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AcivitySession = table.Column<int>(nullable: false),
                    ActivityDifficulty = table.Column<int>(nullable: false),
                    ContentBlockId = table.Column<Guid>(nullable: false),
                    Course = table.Column<int>(nullable: false),
                    Grade = table.Column<float>(nullable: false),
                    SendAt = table.Column<DateTime>(nullable: false),
                    Session = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentAnswer", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentAnswer");
        }
    }
}
