using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Schools.Migrations
{
    public partial class SessionBlocked : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_SessionGroup_GroupId",
                table: "SessionGroup");

            migrationBuilder.DropColumn(
                name: "IsBlocked",
                table: "SessionGroup");

            migrationBuilder.AlterColumn<string>(
                name: "SubjectKey",
                table: "SessionGroup",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LanguageKey",
                table: "SessionGroup",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "Index_SessionBlocked_UniqueSessionBlocked",
                table: "SessionGroup",
                columns: new[] { "GroupId", "LanguageKey", "SubjectKey", "Course", "Session" },
                unique: true,
                filter: "[LanguageKey] IS NOT NULL AND [SubjectKey] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_SessionBlocked_UniqueSessionBlocked",
                table: "SessionGroup");

            migrationBuilder.AlterColumn<string>(
                name: "SubjectKey",
                table: "SessionGroup",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "LanguageKey",
                table: "SessionGroup",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsBlocked",
                table: "SessionGroup",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_SessionGroup_GroupId",
                table: "SessionGroup",
                column: "GroupId");
        }
    }
}
