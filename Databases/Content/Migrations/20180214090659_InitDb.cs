using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Database.Migrations
{
    public partial class InitDb : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Course",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Number = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Course", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Language",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Code = table.Column<string>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Language", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Subject",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    DifficultyCount = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false),
                    StageCount = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subject", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Transition",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Transition", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Activity",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CourseId = table.Column<Guid>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Difficulty = table.Column<int>(nullable: false),
                    IsTimeDependant = table.Column<bool>(nullable: false),
                    LanguageId = table.Column<Guid>(nullable: false),
                    Session = table.Column<int>(nullable: false),
                    Stage = table.Column<int>(nullable: false),
                    SubjectId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Activity", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Activity_Course_CourseId",
                        column: x => x.CourseId,
                        principalTable: "Course",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Activity_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Activity_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransitionProperty",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ItemState = table.Column<int>(nullable: false),
                    ItemType = table.Column<int>(nullable: false),
                    Property = table.Column<int>(nullable: false),
                    TransitionId = table.Column<Guid>(nullable: false),
                    Value = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransitionProperty", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransitionProperty_Transition_TransitionId",
                        column: x => x.TransitionId,
                        principalTable: "Transition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Scene",
                columns: table => new
                {
                    ActivityId = table.Column<Guid>(nullable: true),
                    Order = table.Column<int>(nullable: true),
                    Id = table.Column<Guid>(nullable: false),
                    BackgroundImage = table.Column<string>(nullable: true),
                    Discriminator = table.Column<string>(nullable: false),
                    Thumbnail = table.Column<string>(nullable: true),
                    LanguageId = table.Column<Guid>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    SubjectId = table.Column<Guid>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Scene", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Scene_Activity_ActivityId",
                        column: x => x.ActivityId,
                        principalTable: "Activity",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Scene_Language_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Language",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_Scene_Subject_SubjectId",
                        column: x => x.SubjectId,
                        principalTable: "Subject",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Item",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Discriminator = table.Column<string>(nullable: false),
                    Height = table.Column<int>(nullable: false),
                    Image = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Rotation = table.Column<int>(nullable: false),
                    SceneId = table.Column<Guid>(nullable: false),
                    Text = table.Column<string>(nullable: true),
                    Width = table.Column<int>(nullable: false),
                    X = table.Column<int>(nullable: false),
                    Y = table.Column<int>(nullable: false),
                    ZIndex = table.Column<int>(nullable: false),
                    Solution = table.Column<string>(nullable: true),
                    IsCorrect = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Item", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Item_Scene_SceneId",
                        column: x => x.SceneId,
                        principalTable: "Scene",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DragDrop",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ItemDragId = table.Column<Guid>(nullable: false),
                    ItemDropId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DragDrop", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DragDrop_Item_ItemDragId",
                        column: x => x.ItemDragId,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_DragDrop_Item_ItemDropId",
                        column: x => x.ItemDropId,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "Style",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    BackgroundColor = table.Column<string>(nullable: true),
                    BorderColor = table.Column<string>(nullable: true),
                    BorderRadius = table.Column<int>(nullable: false),
                    BorderWidth = table.Column<int>(nullable: false),
                    ItemId = table.Column<Guid>(nullable: false),
                    ShadowColor = table.Column<string>(nullable: true),
                    ShadowHorizontalOffset = table.Column<int>(nullable: false),
                    ShadowOpacity = table.Column<float>(nullable: false),
                    ShadowVerticalOffset = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Style", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Style_Item_ItemId",
                        column: x => x.ItemId,
                        principalTable: "Item",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Activity_LanguageId",
                table: "Activity",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Activity_SubjectId",
                table: "Activity",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "Index_Activity_CourseSubjectStageDifficulty",
                table: "Activity",
                columns: new[] { "CourseId", "SubjectId", "Stage", "Difficulty", "LanguageId", "Session" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_DragDrop_ItemDropId",
                table: "DragDrop",
                column: "ItemDropId");

            migrationBuilder.CreateIndex(
                name: "Index_DragDrop_UniqueDragDrop",
                table: "DragDrop",
                columns: new[] { "ItemDragId", "ItemDropId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Item_SceneId",
                table: "Item",
                column: "SceneId");

            migrationBuilder.CreateIndex(
                name: "IX_Scene_ActivityId",
                table: "Scene",
                column: "ActivityId");

            migrationBuilder.CreateIndex(
                name: "IX_Scene_LanguageId",
                table: "Scene",
                column: "LanguageId");

            migrationBuilder.CreateIndex(
                name: "IX_Scene_SubjectId",
                table: "Scene",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Style_ItemId",
                table: "Style",
                column: "ItemId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransitionProperty_TransitionId",
                table: "TransitionProperty",
                column: "TransitionId");

            migrationBuilder.CreateIndex(
                name: "Index_TransitionProperty_UniqueTransitionProp",
                table: "TransitionProperty",
                columns: new[] { "ItemState", "ItemType", "Property", "TransitionId" },
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DragDrop");

            migrationBuilder.DropTable(
                name: "Style");

            migrationBuilder.DropTable(
                name: "TransitionProperty");

            migrationBuilder.DropTable(
                name: "Item");

            migrationBuilder.DropTable(
                name: "Transition");

            migrationBuilder.DropTable(
                name: "Scene");

            migrationBuilder.DropTable(
                name: "Activity");

            migrationBuilder.DropTable(
                name: "Course");

            migrationBuilder.DropTable(
                name: "Language");

            migrationBuilder.DropTable(
                name: "Subject");
        }
    }
}
