using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Api.Databases.Schools.Migrations
{
    public partial class addGroupUniversal : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [Group] (Id, AccessAllSessions, Course, FirstSessionWithDiagnosis, [Key], LimitCourse, LimitSession, [Name], SchoolId, SubjectKey, LanguageKey) " + 
                "VALUES " +
                "(NEWID(), 1, 1, 1, 'Universal1', 6, 30, 'Universal1', 'Universal', 'emat', 'es-ES'), " +
                "(NEWID(), 1, 2, 1, 'Universal2', 6, 30, 'Universal2', 'Universal', 'emat', 'es-ES'), " +
                "(NEWID(), 1, 3, 1, 'Universal3', 6, 30, 'Universal3', 'Universal', 'emat', 'es-ES'), " +
                "(NEWID(), 1, 4, 1, 'Universal4', 6, 30, 'Universal4', 'Universal', 'emat', 'es-ES'), " +
                "(NEWID(), 1, 5, 1, 'Universal5', 6, 30, 'Universal5', 'Universal', 'emat', 'es-ES'), " +
                "(NEWID(), 1, 6, 1, 'Universal6', 6, 30, 'Universal6', 'Universal', 'emat', 'es-ES')");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [Group] " +
                "WHERE [Key] LIKE 'Universal%'" +
                "AND [Name] LIKE 'Universal%'" +
                "AND SchoolId = 'Universal'" +
                "AND SubjectKey = 'emat'" +
                "AND LanguageKey = 'es-ES'");
        }
    }
}
