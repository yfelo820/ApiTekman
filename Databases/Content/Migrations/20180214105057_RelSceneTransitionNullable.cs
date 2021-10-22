using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Database.Migrations
{
    public partial class RelSceneTransitionNullable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "TransitionId",
                table: "Scene",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Scene_TransitionId",
                table: "Scene",
                column: "TransitionId");

            migrationBuilder.AddForeignKey(
                name: "FK_Scene_Transition_TransitionId",
                table: "Scene",
                column: "TransitionId",
                principalTable: "Transition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Scene_Transition_TransitionId",
                table: "Scene");

            migrationBuilder.DropIndex(
                name: "IX_Scene_TransitionId",
                table: "Scene");

            migrationBuilder.DropColumn(
                name: "TransitionId",
                table: "Scene");
        }
    }
}
