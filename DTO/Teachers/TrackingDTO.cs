using System;
using System.Collections.Generic;
using Api.Entities.Schools;
using Newtonsoft.Json;

namespace Api.DTO.Teachers
{
    public class GroupTrackingDTO
    {
        public string Key { get; set; }
        public string Name { get; set; }
        public int Course { get; set; }
        public string Language { get; set; }
        public List<StudentTrackingDTO> Students { get; set; }
    }

    public class StudentTrackingDTO
    {
        public string FullName { get; set; }
        public string Photo { get; set; }
        public string UserName { get; set; }
        public int AccessNumber { get; set; }
        public float AverageScore { get; set; }
        public int CompletedSessions { get; set; }
        public int CurrentSession { get; set; }
        public int CurrentCourse { get; set; }
    }

    public class GroupTrackingResponse
    {
        public Guid Id { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
        public int Course { get; set; }
        public string Language { get; set; }
        public IEnumerable<StudentTrackingResponse> Students { get; set; }
    }

    public class StudentTrackingResponse
    {
        public string FullName { get; set; }
        public string Photo { get; set; }
        public string UserName { get; set; }
        public int AccessNumber { get; set; }
        public float AverageScore { get; set; }
        public int CompletedSessions { get; set; }
        public IEnumerable<StudentProgressDto> LanguageProgress { get; set; }
    }

    public class StudentTrackingExtendedDTO : StudentTrackingDTO {
        public string Name { get; set; }
        public string Lastname1 { get; set; }
        public string Lastname2 { get; set; }
        public List<ActivityScore> AverageScores { get; set; }
    }

    public class GroupSessionProgress
    {
        public float AverageScoreSession { get; set; }
        public int AlReadyStarted { get; set; }
        public float ForwardSuccessfulyPercentage { get; set; }
        public float NeededToReviewPercentage { get; set; }
        public float NeededToReinforcePercentage { get; set; }
        public float NotCompleteAllActivitiesPercentage { get; set; }
        public int TotalStudents { get; set; }
        public int MaxSessionSent { get; set; }
    }

    public class StudentResult
    {
        public string FullName { get; set; }
        public string UserName { get; set; }
        public decimal AverageScore { get; set; }
        public int CompletedSessions { get; set; }
        public int CurrentSession { get; set; }
        public int CurrentCourse { get; set; }
    }

    public class ActivityScore
    {
        public Guid Id { get; set; }
        public string ContentBlockName { get; set; }
        public float Score { get; set; }
    }

    public class Grades
    {
        public float MeanGrade;
        public float MaxGrade;
    }

    public class StudentProgressDto
    {
        public string Language { get; }
        public int CurrentCourse { get; }
        public int CurrentSession { get; }

        public StudentProgressDto(string language, int currentCourse, int currentSession)
        {
            Language = language;
            CurrentCourse = currentCourse;
            CurrentSession = currentSession;
        }
    }

    public class StudentsListSubjectsAverage 
    {
        public List<StudentSuperBlocksProgress> studentsList { get; set; }

        public StudentsListSubjectsAverage()
        {
            studentsList = new List<StudentSuperBlocksProgress>();            
        }
    }

    public class StudentSuperBlocksProgress
    {
        public string userName { get; set; }
        public float SuperBlocksAverage { get; set; }
        public List<SuperBlocks> superBlocks { get; set; }

        public StudentSuperBlocksProgress()
        {
            superBlocks = new List<SuperBlocks>();
        }
    }

    public class GroupBlockProgress
    {
        public List<SuperBlocks> SuperBlocks { get; set; }
        public float SuperBlocksAverage { get; set; }

        public GroupBlockProgress()
        {
            SuperBlocks = new List<SuperBlocks>();
        }
    }

    public class SuperBlocks
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public float average { get; set; }
        public List<Blocks> blocks { get; set; }

        public SuperBlocks()
        {
            blocks = new List<Blocks>();                
        }
    }

    public class Blocks
    {
        public Guid id { get; set; }
        public string name { get; set; }
        public float average { get; set; }
        public bool hasAnswers { get; set; }
    }

    public class CoursesPerStudent
    {
        public string userName { get; set; }
        public List<int> courses { get; set; }
        public int currentCourse { get; set; }
        public int currentSession { get; set; }

        public CoursesPerStudent()
        {
            courses = new List<int>();
        }
    }

    public class StagesPerSession
    {
        public string userName { get; set; }
        public int course { get; set; }
        public int session { get; set; }
        public float averageSession { get; set; }
        public List<StagesGrade> stages { get; set; }

        public StagesPerSession()
        {
            stages = new List<StagesGrade>();
        }
    }

    public class StagesGrade
    {
        public float grade { get; set; }
        public int stage { get; set; }
        public string contentBlockName { get; set; }
        public StageState state { get; set; }
        public bool hasAnswers { get; set; }
    }

    public enum StageState
    { 
        Forward = 1,
        Review = 2,
        Reinforce = 3,
        NotComplete = 4
    }
    
    public class ActivitiesOfStage
    {
        public Guid? activityId { get; set; }
        public string name { get; set; }
        public bool passed { get; set; }
        public StageState state { get; set; }
    }

    public class PostStudentProgressDto
    {
        public Guid groupID { get; set; }
        public string userName { get; set; }
        public int session { get; set; }
        public int course { get; set; }
    }

    public class PostStudentGroupDto
    {
        public Guid groupID { get; set; }
        public string userName { get; set; }        
    }
}