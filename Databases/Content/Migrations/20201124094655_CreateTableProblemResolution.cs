using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Databases.Content.Migrations
{
    public partial class CreateTableProblemResolution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProblemResolutionId",
                table: "Activity",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ProblemResolution",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    LanguageId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProblemResolution", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProblemResolution_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_ProblemResolutionId",
                table: "Activity",
                column: "ProblemResolutionId");

            migrationBuilder.CreateIndex(
                name: "IX_ProblemResolution_LanguageId",
                table: "ProblemResolution",
                column: "LanguageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Activity_ProblemResolution_ProblemResolutionId",
                table: "Activity",
                column: "ProblemResolutionId",
                principalTable: "ProblemResolution",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Activity_ProblemResolution_ProblemResolutionId",
                table: "Activity");

            migrationBuilder.DropTable(
                name: "ProblemResolution");

            migrationBuilder.DropIndex(
                name: "IX_Activity_ProblemResolutionId",
                table: "Activity");

            migrationBuilder.DropColumn(
                name: "ProblemResolutionId",
                table: "Activity");
        }
    }
}
