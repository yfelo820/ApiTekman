using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Schools.Migrations
{
    public partial class StudentAnswerAddIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "Index_StudentAnswer_UserNameSubjectLang",
                table: "StudentAnswer",
                columns: new[] { "UserName", "SubjectKey", "LanguageKey" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_StudentAnswer_UserNameSubjectLang",
                table: "StudentAnswer");
        }
    }
}
