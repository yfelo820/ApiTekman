using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Schools.Migrations
{
    public partial class AddDiagnosisTestState : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDiagnosisTestCompleted",
                table: "StudentProgress");

            migrationBuilder.AddColumn<int>(
                name: "DiagnosisTestState",
                table: "StudentProgress",
                nullable: false,
                defaultValue: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DiagnosisTestState",
                table: "StudentProgress");

            migrationBuilder.AddColumn<bool>(
                name: "IsDiagnosisTestCompleted",
                table: "StudentProgress",
                nullable: false,
                defaultValue: false);
        }
    }
}
