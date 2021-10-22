using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Content.Migrations
{
    public partial class MultiDragFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMultiple",
                table: "Item",
                nullable: true);

			// Setting a default of false for isMultiple on the item drags
			migrationBuilder.Sql("UPDATE [item] SET [IsMultiple]=0 WHERE [Discriminator]='ItemDrag'");

            migrationBuilder.AddColumn<int>(
                name: "MultipleDragResult",
                table: "DragDrop",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMultiple",
                table: "Item");

            migrationBuilder.DropColumn(
                name: "MultipleDragResult",
                table: "DragDrop");
        }
    }
}
