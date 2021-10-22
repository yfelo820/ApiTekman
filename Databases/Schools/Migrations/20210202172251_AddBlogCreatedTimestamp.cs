using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Databases.Schools.Migrations
{
    public partial class AddBlogCreatedTimestamp : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentProblemsAnswer",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ActivityContentBlockId = table.Column<Guid>(nullable: false),
                    SubjectKey = table.Column<string>(type: "VARCHAR(20)", maxLength: 20, nullable: true),
                    Course = table.Column<int>(nullable: false),
                    Session = table.Column<int>(nullable: false),
                    Stage = table.Column<int>(nullable: false),
                    UserName = table.Column<string>(type: "VARCHAR(450)", maxLength: 450, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentProblemsAnswer", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentProblemsAnswer");
        }
    }
}
