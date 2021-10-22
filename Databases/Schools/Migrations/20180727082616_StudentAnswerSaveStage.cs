using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Schools.Migrations
{
    public partial class StudentAnswerSaveStage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ContentBlockId",
                table: "StudentAnswer",
                newName: "ActivityContentBlockId");

            migrationBuilder.AddColumn<int>(
                name: "Stage",
                table: "StudentAnswer",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Stage",
                table: "StudentAnswer");

            migrationBuilder.RenameColumn(
                name: "ActivityContentBlockId",
                table: "StudentAnswer",
                newName: "ContentBlockId");
        }
    }
}
