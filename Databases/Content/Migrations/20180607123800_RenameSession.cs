using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Content.Migrations
{
    public partial class RenameSession : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StageCount",
                table: "Subject",
                newName: "SessionCount");

            migrationBuilder.RenameColumn(
                name: "Session",
                table: "Activity",
                newName: "Order");

            migrationBuilder.RenameColumn(
                name: "Stage",
                table: "Activity",
                newName: "Session");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "SessionCount",
                table: "Subject",
                newName: "StageCount");

            migrationBuilder.RenameColumn(
                name: "Session",
                table: "Activity",
                newName: "Stage");

            migrationBuilder.RenameColumn(
                name: "Order",
                table: "Activity",
                newName: "Session");
        }
    }
}
