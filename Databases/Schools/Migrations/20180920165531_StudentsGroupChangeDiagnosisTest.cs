using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Schools.Migrations
{
    public partial class StudentsGroupChangeDiagnosisTest : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "RepeatDiagnosisTest",
                table: "StudentProgress",
                newName: "IsDiagnosisTestCompleted");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsDiagnosisTestCompleted",
                table: "StudentProgress",
                newName: "RepeatDiagnosisTest");
        }
    }
}
