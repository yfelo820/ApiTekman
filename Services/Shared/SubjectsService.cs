using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.DTO.Backoffice.Subject;
using Api.DTO.Students;
using Api.Entities.Content;
using Api.Exceptions;
using Api.Interfaces.Shared;
using SubjectResponse = Api.DTO.Students.SubjectResponse;

namespace Api.Services.Shared
{
    public interface ISubjectsService
    {
        Task<DTO.Backoffice.Subject.SubjectResponse> GetSingle(Guid id);
        Task<DTO.Backoffice.Subject.SubjectResponse> GetSingle(string key);
        Task<List<DTO.Backoffice.Subject.SubjectResponse>> GetAll();
        Task<DTO.Backoffice.Subject.SubjectResponse> Add(SubjectRequest subject);
        Task<DTO.Backoffice.Subject.SubjectResponse> Update(Guid id, SubjectRequest subject);
        Task<DTO.Backoffice.Subject.SubjectResponse> Delete(Guid id);
        Task<SubjectResponse> GetSubject(string subjectKey);
    }

    public class SubjectsService : ISubjectsService
    {
        private readonly IContentRepository<Subject> _subjects;
        public SubjectsService(IContentRepository<Subject> repository) => _subjects = repository;

        public async Task<List<DTO.Backoffice.Subject.SubjectResponse>> GetAll()
        {
            return (await _subjects.GetAll()).OrderBy(c => c.Name).Select(DTO.Backoffice.Subject.SubjectResponse.Map).ToList();
        }

        public async Task<DTO.Backoffice.Subject.SubjectResponse> GetSingle(Guid id)
        {
            return DTO.Backoffice.Subject.SubjectResponse.Map(await _subjects.Get(id));
        }

        public async Task<DTO.Backoffice.Subject.SubjectResponse> GetSingle(string key)
        {
            return DTO.Backoffice.Subject.SubjectResponse.Map(await _subjects.FindSingle(s => string.Equals(s.Key, key)));
        }

        public async Task<DTO.Backoffice.Subject.SubjectResponse> Add(SubjectRequest subject)
        {
            return DTO.Backoffice.Subject.SubjectResponse.Map(await _subjects.Add(SubjectRequest.ToEntity(subject)));
        }

        public async Task<DTO.Backoffice.Subject.SubjectResponse> Update(Guid id, SubjectRequest subject)
        {
            var oldSubject = await _subjects.FindSingle(s => s.Id == id);
            oldSubject.DifficultyCount = subject.DifficultyCount;
            oldSubject.SessionCount = subject.SessionCount;
            oldSubject.Name = subject.Name;
            await _subjects.Update(oldSubject);
            return DTO.Backoffice.Subject.SubjectResponse.Map(oldSubject);
        }

        public async Task<DTO.Backoffice.Subject.SubjectResponse> Delete(Guid id)
        {
            var subject = await _subjects.Get(id);
            await _subjects.Delete(subject);
            return DTO.Backoffice.Subject.SubjectResponse.Map(subject);
        }

        public async Task<SubjectResponse> GetSubject(string subjectKey)
        {
            var entity = await _subjects.FindSingle(s => s.Key == subjectKey);
            if (entity == null)
            {
                throw new NotFoundException("Subject not found");
            }

            var subject = SubjectResponse.Map(entity);
            subject.Courses = GetCourses(entity);
            switch (subjectKey)
            {
                case SubjectKey.Emat:
                    subject.StagesInSession = Config.StageCountInEmatSession;
                    break;
                case SubjectKey.EmatInfantil:
                    subject.StagesInSession = Config.StageCountInEmatInfantilSession;
                    break;
                default:
                    subject.StagesInSession = Config.StageCountInLudiSession;
                    break;
            }

            return subject;
        }

        private IEnumerable<SubjectCourseDTO> GetCourses(Subject subject)
        {
            var courses = new List<SubjectCourseDTO>();

            switch (subject.Key)
            {
                case SubjectKey.Ludi:
                    courses = FillCourses(Config.LudiSessions);
                    break;
                case SubjectKey.LudiCat:
                    courses = FillCourses(Config.LudiCatSessions);
                    break;
                default:
                    var subjectCourses = Config.SubjectCourses[subject.Key];
                    for (var course = subjectCourses.Start; course <= subjectCourses.End; course++)
                    {
                        courses.Add(new SubjectCourseDTO {Number = course, SessionCount = subject.SessionCount});
                    }

                    break;
            }

            return courses;
        }

        private static List<SubjectCourseDTO> FillCourses(int[] courseSessions)
        {
            var courses = new List<SubjectCourseDTO>();
            var course = 1;
            foreach (var sessionCount in courseSessions)
            {
                courses.Add(new SubjectCourseDTO {Number = course, SessionCount = sessionCount});
                course++;
            }

            return courses;
        }
    }
}