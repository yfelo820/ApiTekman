using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Schools.Migrations
{
    public partial class SessionUnblocked_changeName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionBlocked_Group_GroupId",
                table: "SessionBlocked");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionBlocked",
                table: "SessionBlocked");

            migrationBuilder.RenameTable(
                name: "SessionBlocked",
                newName: "SessionUnblocked");

            migrationBuilder.AddPrimaryKey(
                name: "PK_SessionUnblocked",
                table: "SessionUnblocked",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_SessionUnblocked_Group_GroupId",
                table: "SessionUnblocked",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SessionUnblocked_Group_GroupId",
                table: "SessionUnblocked");

            migrationBuilder.DropPrimaryKey(
                name: "PK_SessionUnblocked",
                table: "SessionUnblocked");

            migrationBuilder.RenameTable(
                name: "SessionUnblocked",
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
    }
}
