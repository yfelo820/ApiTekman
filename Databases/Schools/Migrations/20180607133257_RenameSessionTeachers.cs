using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Schools.Migrations
{
    public partial class RenameSessionTeachers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Stage",
                table: "StudentProgress",
                newName: "Session");

            migrationBuilder.RenameColumn(
                name: "LimitStage",
                table: "Group",
                newName: "LimitSession");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Session",
                table: "StudentProgress",
                newName: "Stage");

            migrationBuilder.RenameColumn(
                name: "LimitSession",
                table: "Group",
                newName: "LimitStage");
        }
    }
}
