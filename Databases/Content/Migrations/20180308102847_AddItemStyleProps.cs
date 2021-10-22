using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Database.Migrations
{
    public partial class AddItemStyleProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BorderStyle",
                table: "Style",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsShadowInset",
                table: "Style",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ShadowBlur",
                table: "Style",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BorderStyle",
                table: "Style");

            migrationBuilder.DropColumn(
                name: "IsShadowInset",
                table: "Style");

            migrationBuilder.DropColumn(
                name: "ShadowBlur",
                table: "Style");
        }
    }
}
