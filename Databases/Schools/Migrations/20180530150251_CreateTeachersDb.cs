using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Schools.Migrations
{
    public partial class CreateTeachersDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Group",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AccessAllSessions = table.Column<bool>(nullable: false),
                    Course = table.Column<int>(nullable: false),
                    FirstSessionWithDiagnosis = table.Column<bool>(nullable: false),
                    Key = table.Column<string>(nullable: false),
                    LimitCourse = table.Column<int>(nullable: false),
                    LimitStage = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    SchoolId = table.Column<string>(nullable: false),
                    SubjectKey = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Group", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentProgress",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Course = table.Column<int>(nullable: false),
                    RepeatDiagnosisTest = table.Column<bool>(nullable: false),
                    Stage = table.Column<int>(nullable: false),
                    SubjectKey = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentProgress", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StudentGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AccessNumber = table.Column<int>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentGroup_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "Index_Group_UniqueKey",
                table: "Group",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Index_Group_UniqueGroup",
                table: "Group",
                columns: new[] { "SubjectKey", "Name", "Course" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Index_StudentGroup_UniqueAccessNumber",
                table: "StudentGroup",
                columns: new[] { "GroupId", "AccessNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Index_StudentGroup_UniqueStudentGroup",
                table: "StudentGroup",
                columns: new[] { "GroupId", "UserName" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "Index_StudentProgress_UniqueSubject",
                table: "StudentProgress",
                columns: new[] { "UserName", "SubjectKey" },
                unique: true,
                filter: "[UserName] IS NOT NULL AND [SubjectKey] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentGroup");

            migrationBuilder.DropTable(
                name: "StudentProgress");

            migrationBuilder.DropTable(
                name: "Group");
        }
    }
}
