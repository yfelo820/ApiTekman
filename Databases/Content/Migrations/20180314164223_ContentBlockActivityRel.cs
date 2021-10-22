using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Database.Migrations
{
    public partial class ContentBlockActivityRel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ContentBlockId",
                table: "Activity",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Activity_ContentBlockId",
                table: "Activity",
                column: "ContentBlockId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_ContentBlock_ContentBlockId",
                table: "Activity",
                column: "ContentBlockId",
                principalTable: "ContentBlock",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_ContentBlock_ContentBlockId",
                table: "Activity");

            migrationBuilder.DropIndex(
                name: "IX_Activity_ContentBlockId",
                table: "Activity");

            migrationBuilder.DropColumn(
                name: "ContentBlockId",
                table: "Activity");
        }
    }
}
