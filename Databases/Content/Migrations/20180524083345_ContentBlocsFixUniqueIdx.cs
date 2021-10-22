using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Content.Migrations
{
    public partial class ContentBlocsFixUniqueIdx : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_ContentBlock_UniqueOrderForLanguage",
                table: "ContentBlock");

            migrationBuilder.CreateIndex(
                name: "Index_ContentBlock_UniqueOrderForLanguageSubject",
                table: "ContentBlock",
                columns: new[] { "Order", "LanguageId", "SubjectId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_ContentBlock_UniqueOrderForLanguageSubject",
                table: "ContentBlock");

            migrationBuilder.CreateIndex(
                name: "Index_ContentBlock_UniqueOrderForLanguage",
                table: "ContentBlock",
                columns: new[] { "Order", "LanguageId" },
                unique: true);
        }
    }
}
