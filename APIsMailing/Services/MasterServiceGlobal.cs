using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.APIsMailing.Interfaces;
using Api.APIsMailing.Responses;
using Api.Entities.Schools;
using Api.Factories;
using Api.Interfaces.Shared;

namespace Api.APIsMailing.Services
{
    public class MasterServiceGlobal : IMasterServiceGlobal
    {
        private readonly ISchoolsRepository<Group> _groups;
        private readonly ISchoolsRepository<Teacher> _teachers;
        private readonly ISchoolsRepository<StudentGroup> _students;
        private readonly ISchoolsRepository<StudentProgress> _progress;
        private readonly ISchoolsRepository<StudentAnswer> _answers;

        public MasterServiceGlobal(ISchoolsRepository<Group> groups,
                            ISchoolsRepository<Teacher> teachers,
                            ISchoolsRepository<StudentGroup> students,
                            ISchoolsRepository<StudentAnswer> answers,
                            ISchoolsRepository<StudentProgress> progress)
        {
            _groups = groups;
            _teachers = teachers;
            _students = students;
            _answers = answers;
            _progress = progress;
        }

        public async Task<MasterResponseGlobal> GetMasterResponse(Guid groupId)
        {
            var response = new MasterResponseGlobal();

            var group = await _groups.Get(groupId);
            var students = (await _students.Find(b => b.GroupId == groupId)).ToList();
            var studentsUsers = students.Select(b => b.UserName).ToList();
            var studentsProgress = await _progress.Find((b) => studentsUsers.Contains(b.UserName)
                                                            && b.Course == group.Course);
            
            var answers = await _answers.Find(b => studentsUsers.Contains(b.UserName)
                                                && b.SubjectKey == group.SubjectKey
                                                && b.LanguageKey == group.LanguageKey
                                                && b.Course == group.Course);

            var lastSessionCompleteDate = await GetLastSessionComplete(students, answers, group.Course);
            var penultimateSessionComplateDate = "NOT COMPLETE OTHER SESSION";
            if (lastSessionCompleteDate.Session > 1)
            {
                penultimateSessionComplateDate = await GetPenultimateSessionComplete(students, answers, group.Course, lastSessionCompleteDate.Session - 1);
            }

            return new MasterResponseGlobal
            {
                SchoolId = group.SchoolId,
                GroupId = groupId,
                GroupName = group.Name,
                StudentsCount = students.Count,
                LastSessionStarted = studentsProgress.Max(b => b.Session),
                LastSessionCompleteDate = lastSessionCompleteDate.Date,
                PenultimateSessionCompleteDate = penultimateSessionComplateDate
            };
        }

        private async Task<string> GetPenultimateSessionComplete(List<StudentGroup> studentGroups, List<StudentAnswer> answers, int course, int session)
        {
            var studentUser = studentGroups.Select(b => b.UserName).ToList();

            var progress = await _progress.Find(b => studentUser.Contains(b.UserName)
                                                  && b.Course == course);

            var maxSessionReached = session;

            while (maxSessionReached > 0)
            {
                var completed = ValidateGroupPassedSession(answers.Where(b => b.Session == maxSessionReached).ToList(), studentUser);
                if (completed) return answers.Where(b => b.Session == maxSessionReached)
                                                                          .OrderByDescending(b => b.CreatedAt)
                                                                          .Select(b => b.CreatedAt)
                                                                          .First()
                                                                          .ToString();
                maxSessionReached--;
            }
            return "NOT COMPLETE OTHER SESSION";
        }

        private async Task<DateAndSession> GetLastSessionComplete(List<StudentGroup> studentGroups, List<StudentAnswer> answers, int course)
        {
            var studentUser = studentGroups.Select(b => b.UserName).ToList();

            var progress = await _progress.Find(b => studentUser.Contains(b.UserName)
                                                  && b.Course == course);

            var maxSessionReached = progress.Max(b => b.Session);

            while (maxSessionReached > 0)
            {
                var completed = ValidateGroupPassedSession(answers.Where(b => b.Session == maxSessionReached).ToList(), studentUser);
                if (completed) return new DateAndSession
                {
                    Date = answers.Where(b => b.Session == maxSessionReached)
                                                                          .OrderByDescending(b => b.CreatedAt)
                                                                          .Select(b => b.CreatedAt)
                                                                          .First()
                                                                          .ToString(),
                    Session = maxSessionReached
                };
                maxSessionReached--;
            }
            return new DateAndSession
            {
                Date = "NO COMPLETE ANY SESSION",
                Session = 0
            };
        }

        private bool ValidateGroupPassedSession(List<StudentAnswer> studentAnswers, List<string> studentsUsers)
        {
            var studentAdvanceSession = 0;

            foreach (var student in studentsUsers)
            {
                var answers = studentAnswers.Where(b => b.UserName == student).OrderByDescending(b => b.CreatedAt);

                if (answers.Any())
                {
                    var studentAnwserNotRepeatStage = new List<StudentAnswer>();
                    foreach (var stdAnswer in answers)
                    {
                        if (studentAnwserNotRepeatStage.Where(b => b.Stage == stdAnswer.Stage).Count() == 0)
                            studentAnwserNotRepeatStage.Add(stdAnswer);
                    }

                    var sessionFactory = new CompleteSessionCalculatorFactory();
                    var sesssionCalculator = sessionFactory.Create(answers.First().SubjectKey);
                    var passesAllStagesSession = sesssionCalculator.CompletedSession(studentAnwserNotRepeatStage);

                    studentAdvanceSession += (passesAllStagesSession) ? 1 : 0;
                }
            }
            return (studentAdvanceSession > studentsUsers.Count / 2);
        }

        public async Task<List<GroupDto>> GetAllEmatGroupDetails()
        {
            var groupsEmat = await _groups.Find(b => b.SubjectKey == "emat");
            var response = new List<GroupDto>();

            foreach (var group in groupsEmat)
                response.Add(new GroupDto
                {
                    Course = group.Course,
                    Id = group.Id,
                    Name = group.Name,
                    SchoolId = group.SchoolId
                });
            return response;
        }
    }
}