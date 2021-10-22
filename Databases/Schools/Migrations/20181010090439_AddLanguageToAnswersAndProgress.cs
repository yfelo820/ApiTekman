using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Schools.Migrations
{
    public partial class AddLanguageToAnswersAndProgress : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LanguageKey",
                table: "StudentProgress",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LanguageKey",
                table: "StudentAnswer",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LanguageKey",
                table: "StudentProgress");

            migrationBuilder.DropColumn(
                name: "LanguageKey",
                table: "StudentAnswer");
        }
    }
}
