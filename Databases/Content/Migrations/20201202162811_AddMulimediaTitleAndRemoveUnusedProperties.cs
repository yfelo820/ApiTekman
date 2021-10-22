using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Databases.Content.Migrations
{
    public partial class AddMulimediaTitleAndRemoveUnusedProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Multimedia_ContentBlock_ContentBlockId",
                table: "Multimedia");

            migrationBuilder.DropIndex(
                name: "IX_Multimedia_ContentBlockId",
                table: "Multimedia");

            migrationBuilder.DropColumn(
                name: "ContentBlockId",
                table: "Multimedia");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Multimedia");

            migrationBuilder.DropColumn(
                name: "Difficulty",
                table: "Multimedia");

            migrationBuilder.DropColumn(
                name: "Session",
                table: "Multimedia");

            migrationBuilder.DropColumn(
                name: "ShortDescription",
                table: "Multimedia");

            migrationBuilder.DropColumn(
                name: "Stage",
                table: "Multimedia");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Multimedia",
                type: "VARCHAR(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Multimedia");

            migrationBuilder.AddColumn<Guid>(
                name: "ContentBlockId",
                table: "Multimedia",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Multimedia",
                maxLength: 150,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Difficulty",
                table: "Multimedia",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Session",
                table: "Multimedia",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ShortDescription",
                table: "Multimedia",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Stage",
                table: "Multimedia",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Multimedia_ContentBlockId",
                table: "Multimedia",
                column: "ContentBlockId");

            migrationBuilder.AddForeignKey(
                name: "FK_Multimedia_ContentBlock_ContentBlockId",
                table: "Multimedia",
                column: "ContentBlockId",
                principalTable: "ContentBlock",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
