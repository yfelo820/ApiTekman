using Api.Constants;
using Api.Entities.Content;
using Api.Entities.Schools;
using Api.Exceptions;
using Api.Interfaces.Shared;
using Api.Interfaces.Students;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Api.Services.Students
{
    public class DiagnosisTestService : IDiagnosisTestService
	{
		private readonly IContentRepository<Activity> _activities;
		private readonly IClaimsService _claims;
		private readonly ISchoolsRepository<StudentGroup> _studentGroups;
		private readonly ISchoolsRepository<StudentProgress> _studentProgress;

		public DiagnosisTestService(
			IContentRepository<Activity> activities,
			ISchoolsRepository<StudentGroup> studentGroups,
			ISchoolsRepository<StudentProgress> studentProgress,
			IClaimsService claims
		) {
			_activities = activities;
			_claims = claims;
			_studentGroups = studentGroups;
			_studentProgress = studentProgress;
		}

		public async Task<List<Guid>> Get()
		{
			var subject = _claims.GetSubjectKey();
			var language = _claims.GetLanguageKey();
			var course = await _studentGroups.Query()
				.Where(s =>
					s.UserName == _claims.GetUserName()
					&& s.Group.LanguageKey == language
					&& s.Group.SubjectKey == subject
				)
				.Select(s => s.Group.Course)
				.FirstOrDefaultAsync();
			return await _activities.Query()
				.Where(a =>
					a.IsDiagnosis 
					&& a.Subject.Key == subject
					&& a.Language.Code == language
					&& a.Course.Number == course
				)
				.OrderBy(a => a.Stage)
				.SelectMany(a => a.Exercises)
				.Select(a => a.Id)
				.ToListAsync();		
		}

		public async Task Post(double grade)
		{
			var subjectKey = _claims.GetSubjectKey();
			var username = _claims.GetUserName();
			var languageKey = _claims.GetLanguageKey();
			var session = (int)Math.Ceiling((grade * 10) / 2.0);
			var studentProgress = await _studentProgress.FindSingle(s =>
				s.SubjectKey == subjectKey
				&& s.UserName == username
				&& s.LanguageKey == languageKey
			);
			if (
				subjectKey == SubjectKey.Ludi
                || subjectKey == SubjectKey.LudiCat
				|| studentProgress.DiagnosisTestState == DiagnosisTestState.Completed
			) {
				throw new HttpException(HttpStatusCode.BadRequest);
			}
			var courseNumber = await _studentGroups.Query()
				.Where(s => 
					s.UserName == username 
					&& s.Group.LanguageKey == languageKey
					&& s.Group.SubjectKey == subjectKey
				)
				.Select(s => s.Group.Course)
				.FirstOrDefaultAsync();
			if (studentProgress.Course == courseNumber)
			{
				studentProgress.Session = Math.Max(studentProgress.Session, session);
			}
			else if (studentProgress.Course < courseNumber)
			{
				studentProgress.Course = courseNumber;
				studentProgress.Session = session;
			}
			studentProgress.DiagnosisTestState = DiagnosisTestState.Completed;
			await _studentProgress.Update(studentProgress);
		}
	}
}