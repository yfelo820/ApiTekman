using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Schools.Migrations
{
    public partial class RemoveIsSessionUnblocked : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionUnblocked");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SessionUnblocked",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Course = table.Column<int>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false),
                    Session = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionUnblocked", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionUnblocked_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "Index_SessionUnblocked_UniqueSessionUnblocked",
                table: "SessionUnblocked",
                columns: new[] { "GroupId", "Course", "Session" },
                unique: true);
        }
    }
}
