using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Database.Migrations
{
    public partial class Feedback : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scene_Language_LanguageId",
                table: "Scene");

            migrationBuilder.DropForeignKey(
                name: "FK_Scene_Subject_SubjectId",
                table: "Scene");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Scene",
                newName: "Template_SubjectId");

            migrationBuilder.RenameColumn(
                name: "LanguageId",
                table: "Scene",
                newName: "Template_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_Scene_SubjectId",
                table: "Scene",
                newName: "IX_Scene_Template_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Scene_LanguageId",
                table: "Scene",
                newName: "IX_Scene_Template_LanguageId");

            migrationBuilder.AddColumn<Guid>(
                name: "LanguageId",
                table: "Scene",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Scene",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubjectId",
                table: "Scene",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scene_LanguageId",
                table: "Scene",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Scene_SubjectId",
                table: "Scene",
                column: "SubjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scene_Language_LanguageId",
                table: "Scene",
                column: "LanguageId",
                principalTable: "Language",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Scene_Subject_SubjectId",
                table: "Scene",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Scene_Language_Template_LanguageId",
                table: "Scene",
                column: "Template_LanguageId",
                principalTable: "Language",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Scene_Subject_Template_SubjectId",
                table: "Scene",
                column: "Template_SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scene_Language_LanguageId",
                table: "Scene");

            migrationBuilder.DropForeignKey(
                name: "FK_Scene_Subject_SubjectId",
                table: "Scene");

            migrationBuilder.DropForeignKey(
                name: "FK_Scene_Language_Template_LanguageId",
                table: "Scene");

            migrationBuilder.DropForeignKey(
                name: "FK_Scene_Subject_Template_SubjectId",
                table: "Scene");

            migrationBuilder.DropIndex(
                name: "IX_Scene_LanguageId",
                table: "Scene");

            migrationBuilder.DropIndex(
                name: "IX_Scene_SubjectId",
                table: "Scene");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Scene");

            migrationBuilder.DropColumn(
                name: "Score",
                table: "Scene");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Scene");

            migrationBuilder.RenameColumn(
                name: "Template_SubjectId",
                table: "Scene",
                newName: "SubjectId");

            migrationBuilder.RenameColumn(
                name: "Template_LanguageId",
                table: "Scene",
                newName: "LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_Scene_Template_SubjectId",
                table: "Scene",
                newName: "IX_Scene_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Scene_Template_LanguageId",
                table: "Scene",
                newName: "IX_Scene_LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scene_Language_LanguageId",
                table: "Scene",
                column: "LanguageId",
                principalTable: "Language",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Scene_Subject_SubjectId",
                table: "Scene",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
