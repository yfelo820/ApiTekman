using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Schools.Migrations
{
    public partial class UpdateIdxStudentProgress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_StudentProgress_UniqueSubject",
                table: "StudentProgress");

            migrationBuilder.AlterColumn<string>(
                name: "LanguageKey",
                table: "StudentProgress",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "Index_StudentProgress_UniqueSubject",
                table: "StudentProgress",
                columns: new[] { "UserName", "SubjectKey", "LanguageKey" },
                unique: true,
                filter: "[UserName] IS NOT NULL AND [SubjectKey] IS NOT NULL AND [LanguageKey] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_StudentProgress_UniqueSubject",
                table: "StudentProgress");

            migrationBuilder.AlterColumn<string>(
                name: "LanguageKey",
                table: "StudentProgress",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "Index_StudentProgress_UniqueSubject",
                table: "StudentProgress",
                columns: new[] { "UserName", "SubjectKey" },
                unique: true,
                filter: "[UserName] IS NOT NULL AND [SubjectKey] IS NOT NULL");
        }
    }
}
