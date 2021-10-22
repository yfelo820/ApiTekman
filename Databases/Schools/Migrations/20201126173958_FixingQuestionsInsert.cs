using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Databases.Schools.Migrations
{
    public partial class FixingQuestionsInsert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                                    Update FeedbackQuestionSet SET QuestionLabel1 = 't.infantil.feedback.question1a',
                                                                   QuestionLabel2 = 't.infantil.feedback.question2a',
                                                                   QuestionLabel3 = 't.infantil.feedback.question3a',
                                                                   QuestionLabel4 = 't.infantil.feedback.question4a',
                                                                   QuestionLabel5 = 't.infantil.feedback.question5a' 
                                    WHERE QuestionSetType = 'A'");

            migrationBuilder.Sql(@"
                                    Update FeedbackQuestionSet SET QuestionLabel1 = 't.infantil.feedback.question1b',
                                                                   QuestionLabel2 = 't.infantil.feedback.question2b',
                                                                   QuestionLabel3 = 't.infantil.feedback.question3b',
                                                                   QuestionLabel4 = 't.infantil.feedback.question4b',
                                                                   QuestionLabel5 = 't.infantil.feedback.question5b' 
                                    WHERE QuestionSetType = 'B'");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
