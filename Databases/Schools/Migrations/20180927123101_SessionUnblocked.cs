using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Schools.Migrations
{
    public partial class SessionUnblocked : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "Index_SessionBlocked_UniqueSessionBlocked",
                table: "SessionBlocked",
                newName: "Index_SessionUnblocked_UniqueSessionUnblocked");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameIndex(
                name: "Index_SessionUnblocked_UniqueSessionUnblocked",
                table: "SessionBlocked",
                newName: "Index_SessionBlocked_UniqueSessionBlocked");
        }
    }
}
