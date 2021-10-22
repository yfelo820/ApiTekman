using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Databases.Content.Migrations
{
    public partial class AddSuperBlockContent : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "SuperContentBlockId",
                table: "ContentBlock",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SuperContentBlock",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Order = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true),
                    LanguageId = table.Column<Guid>(nullable: false),
                    SubjectId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SuperContentBlock", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SuperContentBlock_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_SuperContentBlock_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ContentBlock_SuperContentBlockId",
                table: "ContentBlock",
                column: "SuperContentBlockId");

            migrationBuilder.CreateIndex(
                name: "IX_SuperContentBlock_LanguageId",
                table: "SuperContentBlock",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_SuperContentBlock_SubjectId",
                table: "SuperContentBlock",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_ContentBlock_SuperContentBlock_SuperContentBlockId",
                table: "ContentBlock",
                column: "SuperContentBlockId",
                principalTable: "SuperContentBlock",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ContentBlock_SuperContentBlock_SuperContentBlockId",
                table: "ContentBlock");

            migrationBuilder.DropTable(
                name: "SuperContentBlock");

            migrationBuilder.DropIndex(
                name: "IX_ContentBlock_SuperContentBlockId",
                table: "ContentBlock");

            migrationBuilder.DropColumn(
                name: "SuperContentBlockId",
                table: "ContentBlock");
        }
    }
}
