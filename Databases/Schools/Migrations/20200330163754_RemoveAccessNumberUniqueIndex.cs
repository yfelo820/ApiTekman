using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Schools.Migrations
{
    public partial class RemoveAccessNumberUniqueIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_StudentGroup_UniqueAccessNumber",
                table: "StudentGroup");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "Index_StudentGroup_UniqueAccessNumber",
                table: "StudentGroup",
                columns: new[] { "GroupId", "AccessNumber" },
                unique: true);
        }
    }
}
