using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Schools.Migrations
{
    public partial class RenameStudentAnswerActivitySession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AcivitySession",
                table: "StudentAnswer",
                newName: "ActivitySession");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ActivitySession",
                table: "StudentAnswer",
                newName: "AcivitySession");
        }
    }
}
