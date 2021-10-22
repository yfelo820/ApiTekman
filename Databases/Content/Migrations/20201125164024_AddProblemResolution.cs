using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Databases.Content.Migrations
{
    public partial class AddProblemResolution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"Declare @Language_Es uniqueidentifier;
            Declare @Language_Ca uniqueidentifier;
            Declare @Language_Mx uniqueidentifier;

            IF(EXISTS(SELECT [Id] FROM [Language] WHERE [Code] = 'es-ES'))
            BEGIN
            SET @Language_Es = (SELECT
                [Id]

            FROM
                [Language]

            WHERE
                [Code] = 'es-ES');

            INSERT INTO [ProblemResolution] ([Id], [Name], [LanguageId])

            VALUES(NEWID(), 'Cálculo mental', @Language_Es)

            INSERT INTO [ProblemResolution] ([Id], [Name], [LanguageId])

            VALUES(NEWID(), 'Matemáticas para el día a día', @Language_Es)
            END

            IF(EXISTS(SELECT [Id] FROM [Language] WHERE [Code] = 'ca-ES'))
            BEGIN
            SET @Language_Ca = (SELECT
                [Id]

            FROM
                [Language]

            WHERE
                [Code] = 'ca-ES');

            INSERT INTO [ProblemResolution] ([Id], [Name], [LanguageId])

            VALUES(NEWID(), 'Càlcul mental', @Language_Ca)

            INSERT INTO [ProblemResolution] ([Id], [Name], [LanguageId])

            VALUES(NEWID(), 'Matemàtiques per al dia a dia', @Language_Ca)
            END

            IF(EXISTS(SELECT [Id] FROM [Language] WHERE [Code] = 'es-MX'))
            BEGIN
            SET @Language_Mx = (SELECT
                [Id]

            FROM
                [Language]

            WHERE
                [Code] = 'es-MX');

            INSERT INTO [ProblemResolution] ([Id], [Name], [LanguageId])

            VALUES(NEWID(), 'Cálculo mental', @Language_Mx)

            INSERT INTO [ProblemResolution] ([Id], [Name], [LanguageId])

            VALUES(NEWID(), 'Matemáticas para el día a día', @Language_Mx);
            END");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("@DELETE FROM [ProblemResolution]");
        }
    }
}
