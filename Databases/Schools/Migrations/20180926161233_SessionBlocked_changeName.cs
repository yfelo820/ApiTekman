using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Schools.Migrations
{
    public partial class SessionBlocked_changeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionGroup_Group_GroupId",
                table: "SessionGroup");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionGroup",
                table: "SessionGroup");

            migrationBuilder.RenameTable(
                name: "SessionGroup",
                newName: "SessionBlocked");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionBlocked",
                table: "SessionBlocked",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionBlocked_Group_GroupId",
                table: "SessionBlocked",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionBlocked_Group_GroupId",
                table: "SessionBlocked");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionBlocked",
                table: "SessionBlocked");

            migrationBuilder.RenameTable(
                name: "SessionBlocked",
                newName: "SessionGroup");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionGroup",
                table: "SessionGroup",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionGroup_Group_GroupId",
                table: "SessionGroup",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
