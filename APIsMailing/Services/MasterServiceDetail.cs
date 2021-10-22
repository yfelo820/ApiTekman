using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.APIsMailing.Interfaces;
using Api.APIsMailing.Responses;
using Api.Constants;
using Api.Entities.Content;
using Api.Entities.Schools;
using Api.Factories;
using Api.Interfaces.Shared;

namespace Api.APIsMailing.Services
{
    public class MasterServiceDetail : IMasterServiceDetail
    {
        private readonly ISchoolsRepository<Group> _groups;
        private readonly ISchoolsRepository<Teacher> _teachers;
        private readonly ISchoolsRepository<StudentGroup> _students;
        private readonly ISchoolsRepository<StudentAnswer> _answers;
        private readonly IContentRepository<Activity> _activities;
        private readonly IContentRepository<Subject> _subjects;
        private readonly IContentRepository<Language> _languages;
        private readonly IContentRepository<Course> _courses;
        private List<ActivitiesAndState> Activities { get; set; }

        public MasterServiceDetail(ISchoolsRepository<Group> groups,
                             ISchoolsRepository<Teacher> teachers,
                             ISchoolsRepository<StudentGroup> students,
                             ISchoolsRepository<StudentAnswer> answers,
                             IContentRepository<Activity> activities,
                             IContentRepository<Subject> subjects,
                             IContentRepository<Language> languages,
                             IContentRepository<Course> courses)
        {
            _groups = groups;
            _teachers = teachers;
            _students = students;
            _answers = answers;
            _activities = activities;
            _subjects = subjects;
            _languages = languages;
            _courses = courses;
            Activities = new List<ActivitiesAndState>();
        }

        public async Task<MasterResponseDetail> GetMasterResponse(Guid groupId, string username, int session)
        {
            var groupNameSubjectCourse = (await _groups.Find(b => b.Id == groupId))
                                       .Select(nameSubjectCourse => new
                                       {
                                           name = nameSubjectCourse.Name,
                                           subject = nameSubjectCourse.SubjectKey,
                                           course = nameSubjectCourse.Course,
                                           language = nameSubjectCourse.LanguageKey
                                       }).First();

            var name = (await _teachers.Find(b => b.Email == username)).Select(b => b.Name + " " + b.Surnames).FirstOrDefault();
            var students = (await _students.Find(b => b.GroupId == groupId));
            var activities = await GetActivities(session, groupNameSubjectCourse.course, groupNameSubjectCourse.subject, groupNameSubjectCourse.language);
            var detailsStudentList = await GetQuantityStudentsCompleteSession(students, session, groupNameSubjectCourse.course, groupNameSubjectCourse.subject, groupNameSubjectCourse.language);

            return new MasterResponseDetail
            {
                GroupName = groupNameSubjectCourse.name.ToUpper(),
                SubjectName = groupNameSubjectCourse.subject.ToUpper(),
                UserName = name.ToUpper(),
                StudentsCount = students.Count,
                Session = session,
                Course = groupNameSubjectCourse.course,
                Activities = activities,
                QuantityStudentAdvance = detailsStudentList.Quantity,
                StudentNeededReInforce = detailsStudentList.StudentNeededReInforce,
                StudentNeverStartSession = detailsStudentList.StudentNeverStartSession,
                StudentStillInSession = detailsStudentList.StudentStillInSession
            };
        }

        private async Task<List<ActivitiesAndState>> GetActivities(int session, int course, string subjectKey, string languageKey)
        {
            var subjectId = (await _subjects.Find(b => b.Key == subjectKey)).Select(b => b.Id).First();
            var languageId = (await _languages.Find(b => b.Code == languageKey)).Select(b => b.Id).First();
            var courseId = (await _courses.Find(b => b.Number == course)).Select(b => b.Id).First();

            var activities = (await _activities.Find(b => b.SubjectId == subjectId
                                            && b.LanguageId == languageId
                                            && b.CourseId == courseId
                                            && b.Session == session))
                                            .OrderByDescending(b => b.Difficulty)
                                            .ThenBy(b => b.Stage)
                                            .GroupBy(b => b.Stage);

            foreach (var activity in activities)
            {
                Activities.Add(new ActivitiesAndState { Activity = activity.Select(b => b.ShortDescription).First() });
            }
            return Activities;
        }

        private async Task<DetailListStudentAndCountPasses> GetQuantityStudentsCompleteSession(List<StudentGroup> students, int session, int course, string subjectKey, string languageKey)
        {
            var studentsUsers = students.Select(b => b.UserName).ToList();
            var answers = await _answers.Find((b) => studentsUsers.Contains(b.UserName)
                                                  && b.SubjectKey == subjectKey
                                                  && b.LanguageKey == languageKey
                                                  && b.Course == course
                                                  && b.Session == session);

            var quantity = 0;
            var stagesForForwardSuccessfuly = 0;

            var studentNeededReInforce = new List<string>();
            var studentStillInSession = new List<string>();
            var studentNeverStartSession = new List<string>();

            if (!answers.Any())
            {
                quantity = 0;
                studentNeverStartSession.AddRange(students.Select(b => b.UserName));
            }

            else
            {
                foreach (var student in studentsUsers)
                {
                    var studentAnswers = answers.Where(b => b.UserName == student).OrderByDescending(b => b.CreatedAt);

                    if (studentAnswers.Any())
                    {
                        var studentAnwserNotRepeatStage = new List<StudentAnswer>();
                        foreach (var stdAnswer in studentAnswers)
                        {
                            if (studentAnwserNotRepeatStage.Where(b => b.Stage == stdAnswer.Stage).Count() == 0)
                                studentAnwserNotRepeatStage.Add(stdAnswer);
                        }

                        var sessionFactory = new CompleteSessionCalculatorFactory();
                        var sesssionCalculator = sessionFactory.Create(subjectKey);
                        var passesAllStagesSession = sesssionCalculator.CompletedSession(studentAnwserNotRepeatStage);
                        stagesForForwardSuccessfuly = sesssionCalculator.StagesForForwardSuccessfuly();

                        quantity += (passesAllStagesSession) ? 1 : 0;
                        if (!passesAllStagesSession) studentStillInSession.Add(student);
                        if (studentAnswers.GroupBy(b => b.Stage).Any(b => b.Count() > 2)) studentNeededReInforce.Add(student);
                    }
                    else studentNeverStartSession.Add(student);
                }
            }

            GetPercentageByStage(answers, studentsUsers.Count);

            return new DetailListStudentAndCountPasses
            {
                Quantity = quantity,
                StudentNeededReInforce = studentNeededReInforce,
                StudentNeverStartSession = studentNeverStartSession,
                StudentStillInSession = studentStillInSession
            };
        }

        private void GetPercentageByStage(List<StudentAnswer> answers, int count)
        {
            var groupingAnswers = answers.GroupBy(b => b.Stage);

            float countAdvance = 0;
            float countReView = 0;
            float countReInforce = 0;

            foreach (var groupAnswers in groupingAnswers)
            {
                var stageAnswersByUser = groupAnswers.Select(b => b).GroupBy(b => b.UserName);
                foreach (var stageUserAnswers in stageAnswersByUser)
                {
                    var userAnswers = stageUserAnswers.Select(b => b);
                    if (userAnswers.Count() == 1 && userAnswers.First().Grade >= Config.PassGrade) countAdvance++;
                    else if (userAnswers.Count() == 2 && userAnswers.OrderByDescending(b => b.CreatedAt).First().Grade >= Config.PassGrade) countReView++;
                    else if (userAnswers.Count() > 2 && userAnswers.OrderByDescending(b => b.CreatedAt).First().Grade >= Config.PassGrade) countReInforce++;
                }

                Activities[groupAnswers.Key - 1].AdvancePercentage = (countAdvance / count) * 100;
                Activities[groupAnswers.Key - 1].ReInforcePercentage = (countReInforce / count) * 100;
                Activities[groupAnswers.Key - 1].ReViewPercentage = (countReView / count) * 100;

                countAdvance = 0;
                countReInforce = 0;
                countReView = 0;
            }
        }
    }
}