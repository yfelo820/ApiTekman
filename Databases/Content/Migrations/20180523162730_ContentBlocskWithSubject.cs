using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Content.Migrations
{
    public partial class ContentBlocskWithSubject : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "ContentBlock",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_ContentBlock_SubjectId",
                table: "ContentBlock",
                column: "SubjectId");

			// All content blocks by default will be related to the first subject of the database.
			migrationBuilder.Sql(
				"UPDATE [ContentBlock] set SubjectId = (SELECT TOP(1) Id From [Subject])"
			);
			
            migrationBuilder.AddForeignKey(
                name: "FK_ContentBlock_Subject_SubjectId",
                table: "ContentBlock",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentBlock_Subject_SubjectId",
                table: "ContentBlock");

            migrationBuilder.DropIndex(
                name: "IX_ContentBlock_SubjectId",
                table: "ContentBlock");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "ContentBlock");
        }
    }
}
