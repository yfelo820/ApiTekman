using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Schools.Migrations
{
    public partial class SessionUnblocked_alter : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_SessionUnblocked_UniqueSessionUnblocked",
                table: "SessionUnblocked");

            migrationBuilder.DropColumn(
                name: "LanguageKey",
                table: "SessionUnblocked");

            migrationBuilder.DropColumn(
                name: "SubjectKey",
                table: "SessionUnblocked");

            migrationBuilder.CreateIndex(
                name: "Index_SessionUnblocked_UniqueSessionUnblocked",
                table: "SessionUnblocked",
                columns: new[] { "GroupId", "Course", "Session" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_SessionUnblocked_UniqueSessionUnblocked",
                table: "SessionUnblocked");

            migrationBuilder.AddColumn<string>(
                name: "LanguageKey",
                table: "SessionUnblocked",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubjectKey",
                table: "SessionUnblocked",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "Index_SessionUnblocked_UniqueSessionUnblocked",
                table: "SessionUnblocked",
                columns: new[] { "GroupId", "LanguageKey", "SubjectKey", "Course", "Session" },
                unique: true,
                filter: "[LanguageKey] IS NOT NULL AND [SubjectKey] IS NOT NULL");
        }
    }
}
