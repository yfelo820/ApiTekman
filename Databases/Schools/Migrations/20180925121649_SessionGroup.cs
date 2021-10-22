using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Schools.Migrations
{
    public partial class SessionGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SessionGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Course = table.Column<int>(nullable: false),
                    GroupId = table.Column<Guid>(nullable: false),
                    IsBlocked = table.Column<bool>(nullable: false),
                    LanguageKey = table.Column<string>(nullable: true),
                    Session = table.Column<int>(nullable: false),
                    SubjectKey = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SessionGroup", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SessionGroup_Group_GroupId",
                        column: x => x.GroupId,
                        principalTable: "Group",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_SessionGroup_GroupId",
                table: "SessionGroup",
                column: "GroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SessionGroup");
        }
    }
}
