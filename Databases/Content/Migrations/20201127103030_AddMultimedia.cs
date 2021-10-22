using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Databases.Content.Migrations
{
    public partial class AddMultimedia : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Multimedia",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CourseId = table.Column<Guid>(nullable: false),
                    SubjectId = table.Column<Guid>(nullable: false),
                    LanguageId = table.Column<Guid>(nullable: false),
                    ContentBlockId = table.Column<Guid>(nullable: true),
                    Session = table.Column<int>(nullable: false),
                    Difficulty = table.Column<int>(nullable: false),
                    Stage = table.Column<int>(nullable: false),
                    Description = table.Column<string>(maxLength: 150, nullable: true),
                    ShortDescription = table.Column<string>(maxLength: 50, nullable: true),
                    Type = table.Column<byte>(nullable: false),
                    FileName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Multimedia", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Multimedia_ContentBlock_ContentBlockId",
                        column: x => x.ContentBlockId,
                        principalTable: "ContentBlock",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Multimedia_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Multimedia_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Multimedia_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Multimedia_ContentBlockId",
                table: "Multimedia",
                column: "ContentBlockId");

            migrationBuilder.CreateIndex(
                name: "IX_Multimedia_CourseId",
                table: "Multimedia",
                column: "CourseId");

            migrationBuilder.CreateIndex(
                name: "IX_Multimedia_LanguageId",
                table: "Multimedia",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Multimedia_SubjectId",
                table: "Multimedia",
                column: "SubjectId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Multimedia");
        }
    }
}
