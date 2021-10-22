using Microsoft.EntityFrameworkCore.Migrations;

namespace Api.Databases.Schools.Migrations
{
    public partial class AddIncrementalAccessNumber : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DECLARE @UniversalCourses TABLE (Value VARCHAR(10))
                INSERT INTO @UniversalCourses VALUES ('Universal1')
                INSERT INTO @UniversalCourses VALUES ('Universal2')
                INSERT INTO @UniversalCourses VALUES ('Universal3')
                INSERT INTO @UniversalCourses VALUES ('Universal4')
                INSERT INTO @UniversalCourses VALUES ('Universal5')
                INSERT INTO @UniversalCourses VALUES ('Universal6')

                UPDATE s 
                SET s.[AccessNumber] = s.[NewAccessNumber]
                FROM (
	                 SELECT [AccessNumber], ROW_NUMBER() OVER (PARTITION BY g.Id ORDER BY [UserName]) -1 AS NewAccessNumber
	                 FROM [dbo].[Group] g
	                 JOIN [dbo].[StudentGroup] sg ON g.[Id] = sg.[GroupId]
	                 WHERE g.[key] IN (SELECT * FROM @UniversalCourses)
                 ) s");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DECLARE @UniversalCourses TABLE (Value VARCHAR(10))
                INSERT INTO @UniversalCourses VALUES ('Universal1')
                INSERT INTO @UniversalCourses VALUES ('Universal2')
                INSERT INTO @UniversalCourses VALUES ('Universal3')
                INSERT INTO @UniversalCourses VALUES ('Universal4')
                INSERT INTO @UniversalCourses VALUES ('Universal5')
                INSERT INTO @UniversalCourses VALUES ('Universal6')

                UPDATE sg
                SET [AccessNumber] = 0

                FROM [dbo].[Group] g
                JOIN [dbo].[StudentGroup] sg ON g.[Id] = sg.[GroupId]
                WHERE g.[key] IN (SELECT * FROM @UniversalCourses)");
        }
    }
}
