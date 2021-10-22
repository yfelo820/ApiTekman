using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Database.Migrations
{
    public partial class AddAchievements : Migration
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
                name: "Name",
                table: "Scene",
                newName: "Template_Name");

            migrationBuilder.RenameColumn(
                name: "SubjectId",
                table: "Scene",
                newName: "Feedback_SubjectId");

            migrationBuilder.RenameColumn(
                name: "LanguageId",
                table: "Scene",
                newName: "Feedback_LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_Scene_SubjectId",
                table: "Scene",
                newName: "IX_Scene_Feedback_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Scene_LanguageId",
                table: "Scene",
                newName: "IX_Scene_Feedback_LanguageId");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "Scene",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LanguageId",
                table: "Scene",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "Scene",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Session",
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
                name: "FK_Scene_Language_Feedback_LanguageId",
                table: "Scene",
                column: "Feedback_LanguageId",
                principalTable: "Language",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);

            migrationBuilder.AddForeignKey(
                name: "FK_Scene_Subject_Feedback_SubjectId",
                table: "Scene",
                column: "Feedback_SubjectId",
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
                name: "FK_Scene_Language_Feedback_LanguageId",
                table: "Scene");

            migrationBuilder.DropForeignKey(
                name: "FK_Scene_Subject_Feedback_SubjectId",
                table: "Scene");

            migrationBuilder.DropIndex(
                name: "IX_Scene_LanguageId",
                table: "Scene");

            migrationBuilder.DropIndex(
                name: "IX_Scene_SubjectId",
                table: "Scene");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Scene");

            migrationBuilder.DropColumn(
                name: "LanguageId",
                table: "Scene");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "Scene");

            migrationBuilder.DropColumn(
                name: "Session",
                table: "Scene");

            migrationBuilder.DropColumn(
                name: "SubjectId",
                table: "Scene");

            migrationBuilder.RenameColumn(
                name: "Template_Name",
                table: "Scene",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "Feedback_SubjectId",
                table: "Scene",
                newName: "SubjectId");

            migrationBuilder.RenameColumn(
                name: "Feedback_LanguageId",
                table: "Scene",
                newName: "LanguageId");

            migrationBuilder.RenameIndex(
                name: "IX_Scene_Feedback_SubjectId",
                table: "Scene",
                newName: "IX_Scene_SubjectId");

            migrationBuilder.RenameIndex(
                name: "IX_Scene_Feedback_LanguageId",
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
