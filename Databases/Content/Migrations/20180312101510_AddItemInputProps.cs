using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Database.Migrations
{
    public partial class AddItemInputProps : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsIgnoringAccents",
                table: "Item",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsIgnoringCaps",
                table: "Item",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsNumber",
                table: "Item",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsIgnoringAccents",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "IsIgnoringCaps",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "IsNumber",
                table: "Item");
        }
    }
}
