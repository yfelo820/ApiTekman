using System.Collections.Generic;

namespace Api.Constants
{
    public static class Config
    {
        public const string DefaultTkReportsUser = "tkreports@tekmanbooks.com";

        public const float DescendGrade = 0.35f; // Grade where a student goes down a difficulty level (LUDI)
        public const float PassGrade = 0.7f; // Minimum grade to consider an activity as passed.

        public const int MaxDifficultyLudi = 3; // Maximum difficulty for LUDI activities.
        public const int DefaultDifficultyLudi = 2; // Default difficulty for LUDI activities.
        public const int MaxDifficultyEmat = 2; // Maximum difficulty for EMAT activities.
        public const int MaxDifficultyEmatInfantil = 1; // Maximum difficulty for EMAT activities.
        public const int MinDifficulty = 1; // Minimum difficulty for all activities.
        public const int MaxDifficultySuperletras = 5; // Maximum difficulty for Superletras activities.
        public const int MaxDifficultySuperletrasCat = 3; // Maximum difficulty for SuperletrasCat activities.
        public const int FeedbackSessionCount = 3; // Number of sessions between parents' feedback

        public const int MaxPreviousSession = 2; // The maximum session skip for adaptative difficulty (EMAT).

        public static readonly int[] LudiSessions = { 30, 21 }; // Number of session per course. Ex: course 1 = 30 sessions, course 2 = 21 sessions
        public static readonly int[] LudiCatSessions = { 20, 21 }; // Number of session per course. Ex: course 1 = 20 sessions

        // Number of stages in a LUDI/EMAT session
        public static readonly int StageCountInEmatSession = 6;
        public static readonly int StageCountInLudiSession = 1;
        public static readonly int StageCountInEmatInfantilSession = 7;

        // SW-4054 [TODO] develop this functionality from the backoffice
        public static readonly Dictionary<string, SubjectCourses> SubjectCourses = new Dictionary<string, SubjectCourses>
        {
            {SubjectKey.Emat, new SubjectCourses(1, 6)},
            {SubjectKey.EmatInfantil, new SubjectCourses(13, 15) },
            {SubjectKey.EmatMx, new SubjectCourses(1, 5)},
            {SubjectKey.Ludi, new SubjectCourses(1, 2)},
            {SubjectKey.LudiCat, new SubjectCourses(1, 2)},
            {SubjectKey.Superletras, new SubjectCourses(3, 6) },
            {SubjectKey.SuperletrasCat, new SubjectCourses(3, 3) }
        };
    }

    public class SubjectCourses
    {
        public SubjectCourses(int start, int end)
        {
            Start = start;
            End = end;
        }

        public int Start { get; }
        public int End { get; }
    }
}
