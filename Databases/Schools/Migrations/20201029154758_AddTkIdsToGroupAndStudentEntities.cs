using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Databases.Schools.Migrations
{
    public partial class AddTkIdsToGroupAndStudentEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TkStudentId",
                table: "StudentGroup",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TkGroupId",
                table: "Group",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TkStudentId",
                table: "StudentGroup");

            migrationBuilder.DropColumn(
                name: "TkGroupId",
                table: "Group");
        }
    }
}
