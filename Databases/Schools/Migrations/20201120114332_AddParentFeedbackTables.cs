using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Databases.Schools.Migrations
{
    public partial class AddParentFeedbackTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FeedbackAnswerSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    QuestionSetId = table.Column<Guid>(nullable: false),
                    FulfillmentDate = table.Column<DateTime>(nullable: false),
                    ParentEmail = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true),
                    QuestionText1 = table.Column<string>(nullable: false),
                    QuestionText2 = table.Column<string>(nullable: false),
                    QuestionText3 = table.Column<string>(nullable: false),
                    QuestionText4 = table.Column<string>(nullable: false),
                    QuestionText5 = table.Column<string>(nullable: false),
                    AnswerValue1 = table.Column<int>(nullable: false),
                    AnswerValue2 = table.Column<int>(nullable: false),
                    AnswerValue3 = table.Column<int>(nullable: false),
                    AnswerValue4 = table.Column<int>(nullable: false),
                    AnswerValue5 = table.Column<int>(nullable: false),
                    Comments = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackAnswerSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FeedbackQuestionSet",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    QuestionSetType = table.Column<string>(nullable: false),
                    QuestionLabel1 = table.Column<string>(nullable: false),
                    QuestionLabel2 = table.Column<string>(nullable: false),
                    QuestionLabel3 = table.Column<string>(nullable: false),
                    QuestionLabel4 = table.Column<string>(nullable: false),
                    QuestionLabel5 = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackQuestionSet", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PendingFeedback",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserName = table.Column<string>(nullable: false),
                    RequestTime = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingFeedback", x => x.Id);
                });

            migrationBuilder.Sql("insert into FeedbackQuestionSet values(" +
                                 "newid()," +
                                 "'A'," +
                                 "'t.infantil.feedback.question1a'," +
                                 "'t.infantil.feedback.question1a'," +
                                 "'t.infantil.feedback.question1a'," +
                                 "'t.infantil.feedback.question1a'," +
                                 "'t.infantil.feedback.question1a')");

            migrationBuilder.Sql("insert into FeedbackQuestionSet values(" +
                                 "newid()," +
                                 "'B'," +
                                 "'t.infantil.feedback.question1b'," +
                                 "'t.infantil.feedback.question1b'," +
                                 "'t.infantil.feedback.question1b'," +
                                 "'t.infantil.feedback.question1b'," +
                                 "'t.infantil.feedback.question1b')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeedbackAnswerSet");

            migrationBuilder.DropTable(
                name: "FeedbackQuestionSet");

            migrationBuilder.DropTable(
                name: "PendingFeedback");
        }
    }
}
