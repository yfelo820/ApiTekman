using Api.Constants;
using Api.DTO.Teachers;
using Api.Entities.Content;
using Api.Entities.Schools;
using Api.Exceptions;
using Api.Interfaces.Teachers;
using Api.Interfaces.Shared;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Api.Factories;

namespace Api.Services.Teachers
{
    public class TrackingService : ITrackingService
    {
        private readonly IUserService _userService;        
        private readonly IContentRepository<ContentBlock> _contentBlocks;
        private readonly IContentRepository<SuperContentBlock> _superCtBlocks;
        private readonly IContentRepository<Activity> _activity;
        private readonly IContentRepository<Course> _courses;
        private readonly IContentRepository<Language> _languages;
        private readonly GroupsService _groupsService;
        private readonly IContentRepository<Subject> _subjects;
        private readonly ISchoolsRepository<StudentProgress> _progress;
        private readonly ISchoolsRepository<StudentGroup> _studentGroups;
        private readonly ISchoolsRepository<StudentAnswer> _studentAnswers;
        private readonly IClaimsService _claims;
        private readonly IHttpContextService _httpContextService;

        public TrackingService(
            IApiService<GroupDTO> groupsService,
            IContentRepository<ContentBlock> contentBlocks,
            IContentRepository<SuperContentBlock> superCtBlocks,
            IContentRepository<Activity> activity,
            IContentRepository<Course> courses,
            IContentRepository<Language> languages,
            IUserService userService,
            IContentRepository<Subject> subjects,
            ISchoolsRepository<StudentGroup> studentGroups,
            ISchoolsRepository<StudentProgress> progress,
            ISchoolsRepository<StudentAnswer> studentAnswers,
            IClaimsService claims,
            IHttpContextService httpContextService
        )
        {
            _groupsService = groupsService as GroupsService;
            _userService = userService;
            _courses = courses;
            _contentBlocks = contentBlocks;
            _superCtBlocks = superCtBlocks;
            _activity = activity;
            _subjects = subjects;
            _languages = languages;
            _studentGroups = studentGroups;
            _progress = progress;
            _studentAnswers = studentAnswers;
            _claims = claims;
            _httpContextService = httpContextService;
        }

        public async Task<GroupTrackingResponse> GetSingle(Guid groupId)
        {
            var group = await _groupsService.GetSingleGroup(groupId);
            // TODO: for better performance, instead of fetching all the students of the school here,
            // we could implement a get by id[] in the userapi and filter students out. Or we could filter
            // students by the course of its group
            var students = _userService.GetAllStudentsByStage();
            var usernames = group.Students.Select(s => s.UserName).ToList();

            var allStudentProgresses = _progress.Find(p =>
                usernames.Contains(p.UserName)
                && p.SubjectKey == group.SubjectKey
            );

            var allStudentAnswers = _studentAnswers.Find(s =>
                s.SubjectKey == group.SubjectKey
                && (s.SubjectKey == SubjectKey.LudiCat || s.SubjectKey == SubjectKey.SuperletrasCat || s.LanguageKey == group.LanguageKey)
                && usernames.Contains(s.UserName)
            );

            await Task.WhenAll(students, allStudentProgresses, allStudentAnswers);

            var studentsTrackingInfo = GetStudentsTrackingInfo(group,students.Result,allStudentAnswers.Result,allStudentProgresses.Result); 

            return new GroupTrackingResponse()
            {   Id = group.Id,
                Key = group.Key,
                Name = group.Name,
                Course = group.Course,
                Students = studentsTrackingInfo,
                Language = group.LanguageKey
            };
        }

        private IEnumerable<StudentTrackingResponse> GetStudentsTrackingInfo(Group group,
            IEnumerable<StudentUserApiDTO> students, IEnumerable<StudentAnswer> studentsAnswers,
            IEnumerable<StudentProgress> allStudentProgresses)
        {
            return group.Students
                .Where(s => students.Any(st => st.UserName == s.UserName))
                .Select(studentGroup =>
                {
                    var student = students
                        .FirstOrDefault(s => studentGroup.UserName == s.UserName);
                    var studentAnswers = studentsAnswers
                        .Where(s => String.Compare(s.UserName, studentGroup.UserName, CultureInfo.CurrentCulture,
                            CompareOptions.IgnoreNonSpace) == 0);
                    var studentProgresses = allStudentProgresses.Where(sp =>
                        sp.UserName == student.UserName);

                    return new StudentTrackingResponse()
                    {
                        FullName = student.FirstSurname + " " + student.SecondSurname + ", " + student.Name,
                        Photo = student.Photo,
                        UserName = student.UserName,
                        AccessNumber = studentGroup.AccessNumber,
                        CompletedSessions = group.SubjectKey == SubjectKey.Emat
                            ? StudentAnswerCalculation.CalculateCompletedEmatSessions(studentAnswers)
                            : StudentAnswerCalculation.CalculateCompletedLudiSessions(studentAnswers),
                        AverageScore = StudentAnswerCalculation.CalculateAverageGrade(studentAnswers),
                        LanguageProgress = studentProgresses
                            .Select(studentProgress => new StudentProgressDto(studentProgress.LanguageKey,
                                studentProgress.Course,
                                studentProgress.Session))
                    };
                }).OrderBy(sp => sp.FullName);
        }

        public async Task<StudentTrackingExtendedDTO> GetSingleStudentDetail(string userName)
        {
            // TODO: check wether clains laguage key works correctly
            var language = await _languages.FindSingle(l => l.Code == _claims.GetLanguageKey());
            var subject = await _subjects.FindSingle(s => s.Key == _claims.GetSubjectKey());
            var calculator = StudentAnswerCalculationFactory.CreateFactory(subject.Key);
            var student = await _userService.GetSingleStudent(userName);

            var studentsAnswers = await _studentAnswers.Find(s =>
                s.SubjectKey == subject.Key
                && (s.SubjectKey == SubjectKey.LudiCat || s.SubjectKey == SubjectKey.SuperletrasCat || s.LanguageKey == language.Code)
                && s.UserName.Equals(userName)
            );
            var progress = await _progress.FindSingle(p =>
                p.UserName.Equals(userName)
                && p.LanguageKey == language.Code
                && p.SubjectKey == subject.Key
            );

            var allContentBlocks = await _contentBlocks.GetAll();
            var contentBlocksBySubjectAndLanguage = allContentBlocks.Where(c => c.SubjectId.Equals(subject.Id) && c.LanguageId.Equals(language.Id)).ToList();
            var activityScoreList = new List<ActivityScore>();
            var contentBlocks = studentsAnswers.Select(s => s.ActivityContentBlockId).Distinct().ToList();
            contentBlocksBySubjectAndLanguage.ForEach(c =>
            {
                activityScoreList.Add(new ActivityScore
                {
                    Id = c.Id,
                    Score = calculator.CalculateAverageGrade(studentsAnswers, c.Id),
                    ContentBlockName = c.Name
                });
            });

            return new StudentTrackingExtendedDTO
            {
                Name = student.Name,
                Lastname1 = student.FirstSurname,
                Lastname2 = student.SecondSurname,
                AverageScores = activityScoreList,
                AverageScore = calculator.CalculateAverageGrade(studentsAnswers),
                CompletedSessions = calculator.CalculateCompletedSessions(studentsAnswers),
                CurrentSession = progress?.Session ?? 1
            };
        }

        public async Task<StudentProgress> UpdateStudentProgress(StudentProgress progress)
        {
            progress.SubjectKey = _httpContextService.GetSubjectFromUri();
            var schoolId = _claims.GetSchoolId();
            var isStudentFromSchool = await _studentGroups.Any(
                sg => sg.UserName == progress.UserName && sg.Group.SchoolId == schoolId
            );
            if (!isStudentFromSchool) throw new HttpException(HttpStatusCode.NotFound);

            var oldProgress = await _progress.Query()
                .AsNoTracking()
                .FirstOrDefaultAsync(p =>
                    p.UserName == progress.UserName
                    && p.SubjectKey == progress.SubjectKey
                    && p.LanguageKey == progress.LanguageKey
                );
            progress.Id = oldProgress.Id;
            if (progress.DiagnosisTestState != DiagnosisTestState.Pending)
            {
                progress.DiagnosisTestState = oldProgress.DiagnosisTestState;
            }
            await _progress.Update(progress);
            return progress;
        }

        public async Task<GroupSessionProgress> GetGroupSessionProgress(Guid groupID, int session)
        {
            var groupSession = await _groupsService.GetSingleGroup(groupID);             
            var studentsUserName = groupSession.Students.Select(b => b.UserName).ToList();
            var studentsProgress = (await _progress.Find((n) => studentsUserName.Contains(n.UserName)
                                                             && n.SubjectKey == groupSession.SubjectKey
                                                             && n.Course == groupSession.Course))
                                                             .ToList(); 

            var maxSessionSent = (studentsProgress.Count > 0) ? studentsProgress.Max(b => b.Session) : 1;            
                
            var studentAnswers    = await _studentAnswers.Find(s => studentsUserName.Contains(s.UserName)                                                          
                                                           && s.SubjectKey  == groupSession.SubjectKey  
                                                           && s.LanguageKey == groupSession.LanguageKey  
                                                           && s.Course == groupSession.Course
                                                           && s.Session == session
                                                         );                      

            var AverageSession           = 0.0f;
            var forwardSuccessfuly       = 0.0f;
            var neededToReinforce        = 0.0f;
            var neededToReview           = 0.0f;
            var alReadyStarted             = 0;
            var notCompleteAllActivities = 0.0f;
            var sessionComplete = 0;

            const int twoAttempts = 2;
            const int threeAttempts = 3;

            foreach (var std in studentsUserName)
            {
                var anwsers = studentAnswers.Where(b => b.UserName == std).ToList();                

                if (anwsers.Count > 0)
                    {   
                        alReadyStarted++;
                    var sessionAnwsersForStudent = anwsers.OrderBy(b => b.Stage)
                                                          .ThenByDescending(b => b.CreatedAt)
                                                          .ToList();

                    var studentAnwserNotRepeatStage = new List<StudentAnswer>();
                    foreach(StudentAnswer stdAnswer in sessionAnwsersForStudent)
                    {
                        if (studentAnwserNotRepeatStage.Where(b=>b.Stage == stdAnswer.Stage).Count() == 0)
                            studentAnwserNotRepeatStage.Add(stdAnswer);
                    }

                    var sessionFactory = new CompleteSessionCalculatorFactory();
                    var sesssionCalculator = sessionFactory.Create(groupSession.SubjectKey);
                    var passesAllStagesSession = sesssionCalculator.CompletedSession(studentAnwserNotRepeatStage);
                    var stagesForForwardSuccessfuly = sesssionCalculator.StagesForForwardSuccessfuly();

                    AverageSession += StudentAnswerCalculation.CalculateAverageGrade(studentAnwserNotRepeatStage);

                    if (passesAllStagesSession)
                    {
                            sessionComplete++;  
                            forwardSuccessfuly += (anwsers.Count == stagesForForwardSuccessfuly) ? 1 : 0;                           

                        var stagesAndAttempts = anwsers.GroupBy(answer => answer.Stage)
                                                           .Select(answerGroup => new { Stage = answerGroup.Key, Attempts = answerGroup.Count() });

                        if (stagesAndAttempts.Any(sa => sa.Attempts >= threeAttempts)) neededToReinforce++;
                        else if (stagesAndAttempts.Any(sa => sa.Attempts == twoAttempts)) neededToReview++;
                    }
                        else
                        {                                                      
                            notCompleteAllActivities++;
                        }  
                 }                                
            }  
                      
            return new GroupSessionProgress
            {
                AlReadyStarted                     = alReadyStarted,
                AverageScoreSession                = (alReadyStarted != 0) ? (AverageSession / alReadyStarted)  : 0,
                ForwardSuccessfulyPercentage       = (alReadyStarted != 0) ? forwardSuccessfuly / alReadyStarted * 100 : 0,
                NeededToReviewPercentage           = (alReadyStarted != 0) ? neededToReview / alReadyStarted * 100 : 0,
                NeededToReinforcePercentage        = (alReadyStarted != 0) ? neededToReinforce / alReadyStarted * 100 : 0,
                NotCompleteAllActivitiesPercentage = (alReadyStarted != 0) ? notCompleteAllActivities / alReadyStarted * 100 : 0,
                TotalStudents                      = studentsUserName.Count(),
                MaxSessionSent                     = maxSessionSent
            };
        }

        public async Task<GroupBlockProgress> GetGroupBlockProgress(Guid groupID)
        {
            var response = new GroupBlockProgress();

            var group = await _groupsService.GetSingleGroup(groupID);
            var languageGroupID = (await _languages.GetAll()).Where(b => b.Code == group.LanguageKey).Select(b => b.Id).SingleOrDefault();
            var subjectID = (await _subjects.GetAll()).Where(b => b.Key == group.SubjectKey).Select(b=>b.Id).SingleOrDefault();
            var studentsUserName = group.Students.Select(b => b.UserName).ToList();

            var superCBs = (await _superCtBlocks.GetAll()).Where(b => b.LanguageId == languageGroupID
                                                                          && b.SubjectId == subjectID).OrderBy(b => b.Order);
            var superCBIds = superCBs.Select(superCB => superCB.Id).ToList();

            var contentsBlocks = (await _contentBlocks.GetAll()).Where(b=> b.SubjectId == subjectID 
                                                                               && b.SuperContentBlockId.HasValue
                                                                               && superCBIds.Contains(b.SuperContentBlockId.Value));

            var answers = (await _studentAnswers.Find(s => studentsUserName.Contains(s.UserName)                                                        
                                                        && s.SubjectKey == group.SubjectKey
                                                        && s.LanguageKey == group.LanguageKey
                                                        && s.Course == group.Course));
            
            foreach (var superBlock in superCBs)
            {
                var blocks = new List<Blocks>();
                var filterContentsBlocks = contentsBlocks.Where(b => b.SuperContentBlockId == superBlock.Id).OrderBy(b => b.Order);
                var countForDivide = 0;

                foreach (var cb in filterContentsBlocks)
                {                   
                    var contentBlockAnswers = answers.Where(b => b.ActivityContentBlockId == cb.Id);
                    countForDivide += (contentBlockAnswers.Count() > 0) ? 1 : 0;
                    blocks.Add(new Blocks
                    {
                        id      = cb.Id,
                        name    = cb.Name,
                        average = StudentAnswerCalculation.CalculateAverageGrade(contentBlockAnswers),
                        hasAnswers = (contentBlockAnswers.Count() > 0)
                    });
                }

                var subTotal = blocks.Sum(b => b.average);

                response.SuperBlocks.Add(new SuperBlocks
                {
                    id      = superBlock.Id,
                    name    = superBlock.Name,
                    blocks  = blocks,
                    average = (countForDivide != 0) ? subTotal / countForDivide : 0
                });                
            }

            var superBlocksWithAnswers = 0;
                superBlocksWithAnswers = response.SuperBlocks.Where(sb => sb.blocks.Any(b => b.hasAnswers == true)).Count();

            response.SuperBlocksAverage = (superBlocksWithAnswers > 0) ? response.SuperBlocks.Sum(b => b.average) / superBlocksWithAnswers : 0;
            return response;
        }

        public async Task<GroupBlockProgress> GetStudentSuperBlockProgress(Guid groupId, string userName)
        {
            var response = new GroupBlockProgress();
            
            var group = await _groupsService.GetSingleGroup(groupId);
            var languageGroupID = (await _languages.GetAll()).Where(b => b.Code == group.LanguageKey).Select(b => b.Id).SingleOrDefault();
            var subjectID = (await _subjects.GetAll()).Where(b => b.Key == group.SubjectKey).Select(b => b.Id).SingleOrDefault();

            var superCBs = (await _superCtBlocks.GetAll()).Where(b => b.LanguageId == languageGroupID
                                                                          && b.SubjectId == subjectID).OrderBy(b => b.Order);
            var superCBIds = superCBs.Select(superCB => superCB.Id).ToList();

            var contentsBlocks = (await _contentBlocks.GetAll()).Where(b => b.SubjectId == subjectID
                                                                               && b.SuperContentBlockId.HasValue
                                                                               && superCBIds.Contains(b.SuperContentBlockId.Value));
            var contentsBlocksIds = contentsBlocks.Select(b => b.Id).ToList();

            var answers = (await _studentAnswers.Find(s => contentsBlocksIds.Contains(s.ActivityContentBlockId)
                                                        && s.UserName == userName
                                                        && s.SubjectKey == group.SubjectKey
                                                        && s.LanguageKey == group.LanguageKey
                                                        && s.Course == group.Course));

            foreach (var superBlock in superCBs)
            {
                var filterContentsBlocks = contentsBlocks.Where(b => b.SuperContentBlockId == superBlock.Id).OrderBy(b => b.Order);
                var blocks = new List<Blocks>();
                var countForDivide = 0;

                foreach (var cb in filterContentsBlocks)
                {
                    var contentBlockAnswers = answers.Where(b => b.ActivityContentBlockId == cb.Id);
                    countForDivide += (contentBlockAnswers.Count() > 0) ? 1 : 0;
                    blocks.Add(new Blocks
                    {
                        id = cb.Id,
                        name = cb.Name,
                        average = StudentAnswerCalculation.CalculateAverageGrade(contentBlockAnswers),
                        hasAnswers = (contentBlockAnswers.Count() > 0)
                    });
                }

                var subTotal = blocks.Sum(b => b.average);

                response.SuperBlocks.Add(new SuperBlocks
                {
                    id = superBlock.Id,
                    name = superBlock.Name,
                    blocks = blocks,
                    average = (countForDivide != 0) ? subTotal / countForDivide : 0
                });
            }

            var superBlocksWithAnswers = 0;
                superBlocksWithAnswers = response.SuperBlocks.Where(sb => sb.blocks.Any(b => b.hasAnswers == true)).Count();

            response.SuperBlocksAverage = (superBlocksWithAnswers > 0) ? response.SuperBlocks.Sum(b => b.average) / superBlocksWithAnswers : 0;
            return response;
        }

        public async Task<StudentsListSubjectsAverage> GetStudentsListSubjectsAverage(Guid groupID)
        {
            var response = new StudentsListSubjectsAverage();

            var group = await _groupsService.GetSingleGroup(groupID);
            var students = group.Students.Select(b => b.UserName);

            foreach(string std in students)
            {
                response.studentsList.Add( new StudentSuperBlocksProgress {
                                             userName = std,
                                          superBlocks = (await GetStudentSuperBlockProgress(groupID,std)).SuperBlocks
                                                                  });
                var superBlocksWithAnswers = 0;
                superBlocksWithAnswers = response.studentsList.Last().superBlocks.Where(sb => sb.blocks.Any(b => b.hasAnswers == true)).Count();
                response.studentsList.Last().SuperBlocksAverage = (superBlocksWithAnswers > 0) ? response.studentsList.Last().superBlocks.Sum(b => b.average) / superBlocksWithAnswers : 0;
            }
            return response;
        }

        private async Task<StudentProgress> GetStudentProgress(Guid groupId, string userName)
        {
            var group = await _groupsService.GetSingleGroup(groupId);

            var studentToUpdateProgress = (await _progress.Find(b => b.UserName == userName
                                                          && b.SubjectKey == group.SubjectKey
                                                          && b.LanguageKey == group.LanguageKey)).SingleOrDefault();

            if (studentToUpdateProgress.SubjectKey == null) throw new HttpException(HttpStatusCode.NotFound);

            return studentToUpdateProgress;
        }

        public async Task<StudentProgress> SetStudentToAnotherSession(Guid groupID, string userName, int session, int course)
        {
            var studentToUpdateProgress = await GetStudentProgress(groupID, userName);            

            studentToUpdateProgress.Session = session;
            studentToUpdateProgress.Course = course;
            await _progress.Update(studentToUpdateProgress);

            return studentToUpdateProgress;
        }

        public async Task<StudentProgress> ResetDiagnosisTestState(Guid groupID, string userName)
        {    
            var studentToResetTest = await GetStudentProgress(groupID, userName);

            studentToResetTest.DiagnosisTestState = DiagnosisTestState.Pending;
            await _progress.Update(studentToResetTest);

            return studentToResetTest;
        }

        public async Task<CoursesPerStudent> GetCoursesPerStudent(Guid groupID, string userName)
        {
            var group = await _groupsService.GetSingleGroup(groupID);

            var courses = (await _studentAnswers.Find(s => s.UserName == userName
                                                        && s.SubjectKey == group.SubjectKey
                                                        && s.LanguageKey == group.LanguageKey))                                                        
                                                        .Select(c=>c.Course)                                                        
                                                        .Distinct()
                                                        .OrderByDescending(c=>c)
                                                        .ToList();

            var studentProgress = await GetStudentProgress(groupID, userName);

            return new CoursesPerStudent { 
                             courses  = courses,
                            userName  = userName,
                       currentCourse  = studentProgress.Course,
                       currentSession = studentProgress.Session
            };
        }
           
        private async Task<string> GetStageContentBlockName(Guid courseId, Guid subjectId, Guid languageId, int session, int stage, int difficulty)
        {
            var contentBlockId = (await _activity.Find(b => b.CourseId == courseId   
                                                         && b.SubjectId == subjectId 
                                                         && b.Session == session
                                                         && b.Stage == stage
                                                         && b.LanguageId == languageId
                                                         && b.Difficulty <= difficulty))  
                                                                                        .OrderByDescending(d=>d.Difficulty)
                                                                                        .First().ContentBlockId;

            var contentBlockName = (await _contentBlocks.Get(contentBlockId.Value)).Name;  

            return contentBlockName;  
        }
          
        private async Task<StagesGrade> GetStageGrade(List<StudentAnswer> stageAnswers)
        {
            var stageAnswersOrdered = stageAnswers.OrderByDescending(b => b.CreatedAt).ToList();

            var contentBlockID = stageAnswersOrdered.Select(b => b.ActivityContentBlockId)
                                             .First();

            var contentBlockName = (await _contentBlocks.GetAll())
                               .Where(b => b.Id == contentBlockID)
                               .Select(b => b.Name)
                               .SingleOrDefault();

            var stateStage = StageState.NotComplete;
            var stageGrade = stageAnswersOrdered.First().Grade;

            if (stageAnswersOrdered.Count == 1 && stageGrade >= Config.PassGrade) stateStage = StageState.Forward;
            else if (stageAnswersOrdered.Count == 2 && stageGrade >= Config.PassGrade) stateStage = StageState.Review;
            else if (stageAnswersOrdered.Count > 2 && stageGrade >= Config.PassGrade) stateStage = StageState.Reinforce;

            return new StagesGrade
            {
                contentBlockName = contentBlockName,
                stage = stageAnswers.First().Stage,
                grade = stageAnswersOrdered.First().Grade,
                state = stateStage,
                hasAnswers = stageAnswers.Any(b=>b.ActivityId.HasValue)
            };
        }
         
        private async Task<StagesGrade> AddStagesWithOutAnswers(int stage, int course, int session, string subjectKey, string languageKey, int difficulty)
        {
            var stageGrade = new StagesGrade();
            var courseId = (await _courses.Find(b => b.Number == course)).Select(b => b.Id).First();
            var subjectId = (await _subjects.Find(b => b.Key == subjectKey)).Select(b => b.Id).First();
            var languageId = (await _languages.Find(b => b.Code == languageKey)).Select(b => b.Id).First();
                         
                stageGrade = new StagesGrade
                {
                    contentBlockName = await GetStageContentBlockName(courseId, subjectId, languageId, session, stage, difficulty),
                    stage = stage,
                    grade = 0.0f,
                    state = StageState.NotComplete,
                    hasAnswers = false
                };
            
            return stageGrade;
        }

        public async Task<StagesPerSession> GetStudentSessionProgress(Guid groupID, string userName, int course, int session)
        {
            var group = await _groupsService.GetSingleGroup(groupID);

            var answers = await _studentAnswers.Find(s => s.UserName == userName
                                                       && s.SubjectKey == group.SubjectKey
                                                       && s.LanguageKey == group.LanguageKey
                                                       && s.Course == course
                                                       && s.Session == session);

            var stages = new List<StagesGrade>();
            var answersForAverage = new List<StudentAnswer>();

            var sessionFactory = new CompleteSessionCalculatorFactory();     
            var sesssionCalculator = sessionFactory.Create(group.SubjectKey);
            var totalStages = sesssionCalculator.StagesForForwardSuccessfuly(); 
            int ciberMaxDifficulty = sesssionCalculator.MaxDifficulty();            

            for (var i = 1; i <= totalStages; i++)
            {
                var answersByStage = answers.Where(b => b.Stage == i).OrderByDescending(b => b.CreatedAt);

                if (answersByStage.Count() > 0)
                {      
                    answersForAverage.Add(answersByStage.First());
                    var stage = await GetStageGrade(answersByStage.ToList());
                    stages.Add(stage);
                }
                else
                {
                    var stageForAdd = await AddStagesWithOutAnswers(i, group.Course, session, group.SubjectKey, group.LanguageKey, ciberMaxDifficulty);
                    stages.Add(stageForAdd);  
                }
            }

            return new StagesPerSession
            {
                course = course,
                session = session,
                averageSession = StudentAnswerCalculation.CalculateAverageGrade(answersForAverage),
                userName = userName,
                stages = stages
            };
        }

        public async Task<List<ActivitiesOfStage>> GetActivitiesOfStage(Guid groupID, string userName, int course, int session, int stage)
        {
            var listOfActivities = new List<ActivitiesOfStage>();

            var group = await _groupsService.GetSingleGroup(groupID);

            var answers = await _studentAnswers.Find(s => s.UserName == userName
                                                       && s.SubjectKey == group.SubjectKey
                                                       && s.LanguageKey == group.LanguageKey
                                                       && s.Course == course
                                                       && s.Session == session
                                                       && s.Stage == stage);

            var orderAnswers = answers.OrderBy(b => b.CreatedAt)
                                      .ToList();

            for(var i=0;i<orderAnswers.Count;i++)
            {
                var nameActivity = "";
                if (orderAnswers[i].ActivityId.HasValue)
                {
                     nameActivity = (await _activity.Get(orderAnswers[i].ActivityId.Value))
                                       .ShortDescription;
                }

                var stateStage = StageState.Forward;
                if (i == 1) stateStage = StageState.Review;
                else if (i > 1) stateStage = StageState.Reinforce;

                listOfActivities.Add( new ActivitiesOfStage
                                                {   activityId = orderAnswers[i].ActivityId,
                                                    state = stateStage,
                                                    name = nameActivity,
                                                    passed = (orderAnswers[i].Grade >= Config.PassGrade)
                                                });
                               
            }
            return listOfActivities;
        }
    }
}