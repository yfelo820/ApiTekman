using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Database.Migrations
{
    public partial class ixContentBlock : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "Index_ContentBlock_UniqueOrderForLanguage",
                table: "ContentBlock",
                columns: new[] { "Order", "LanguageId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "Index_ContentBlock_UniqueOrderForLanguage",
                table: "ContentBlock");
        }
    }
}
