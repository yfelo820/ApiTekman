using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.Entities.Content;
using Api.Entities.Schools;
using Api.Helpers;
using Api.Interfaces.Shared;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Quartz;

namespace Api.Jobs
{
    public class StudentAnswersToTkReportsJob : IJob
	{
		private readonly string loginPath;
		private readonly string ematPath;
		private readonly string ludiPath;
		private readonly int maxRows;

		private readonly ISchoolsRepository<StudentAnswer> _studentAnswers;
		private readonly IContentRepository<Activity> _activities;
		private readonly IContentRepository<Log> _logs;
		private readonly HttpFormClient _form;
		private readonly SubjectLanguageRequest[] requests;

		private readonly string EmatActivityViewerUrl = "https://ciberemat.com/teachers/activity/";
		private readonly string LudiActivityViewerUrl = "https://ciberludiletras.com/teachers/activity/";

		public StudentAnswersToTkReportsJob (
			ISchoolsRepository<StudentAnswer> studentAnswers,
			IContentRepository<Log> logs,
			IContentRepository<Activity> activities,
			IConfiguration config
		) 
		{
			_form = new HttpFormClient(config["TkReports:apiUrl"]);
			_studentAnswers = studentAnswers;
			_activities = activities;
			_logs = logs;

			loginPath  = config["TkReports:loginPath"];
			ematPath = config["TkReports:ematPath"];
			ludiPath = config["TkReports:ludiPath"];
			maxRows = int.Parse(config["TkReports:maxRows"]);

			requests = new SubjectLanguageRequest[] {
				new SubjectLanguageRequest() {
					Language = "es-ES",
					Subject = SubjectKey.Ludi,
					Token = config["TkReports:ludi-es-ES"]
				},
				new SubjectLanguageRequest() {
					Language = "es-ES",
					Subject = SubjectKey.Emat,
					Token = config["TkReports:emat-es-ES"]
				},
				new SubjectLanguageRequest() {
					Language = "ca-ES",
					Subject = SubjectKey.Emat,
					Token = config["TkReports:emat-ca-ES"]
				},
				new SubjectLanguageRequest() {
					Language = "es-MX",
					Subject = SubjectKey.Emat,
					Token = config["TkReports:emat-es-MX"]
				}
			};
		}

		public async Task Execute(IJobExecutionContext context)
		{
			var allStudentAnswers = await _studentAnswers.Query()
				.Where(s => !s.IsSentToTkReports && !string.IsNullOrEmpty(s.LanguageKey))
				.Take(maxRows)
				.ToListAsync();
			if (allStudentAnswers.Count == 0) return;
			var allActivities = await _activities
				.Query(new [] { "Subject", "Language", "Course" })
				.ToListAsync();
			foreach (var request in requests) 
			{
				// Getting the student answers to send for this TkReports API login token
				// (there is a token for each combination of language and subject)
				var studentAnswers = allStudentAnswers.Where(s => 
						s.LanguageKey == request.Language 
						&& s.SubjectKey == request.Subject
					).ToList();
				if (!studentAnswers.Any()) continue;
				// Getting the activities to get the activity url from
				var activities = allActivities.Where(a =>
						a.Language.Code == request.Language 
						&& a.Subject.Key == request.Subject
					).ToList();
				try {
					// Log in to TkReports API to send the student answer data to TkReports
					await LogInToTkReports(request.Token);
				} catch(Exception e)
				{
					await _logs.Add(new Log("Login to tekmanApi failed for " + request.Language));
					throw e;
				}
				try {
					if (request.Subject == SubjectKey.Emat) await SendEmatAnswers(studentAnswers, activities);
					else await SendLudiAnswers(studentAnswers, activities);
				} catch(Exception e)
				{
					await _logs.Add(new Log(
						"Exception in request to tkreports for studentAnswers: "
						+ string.Join(", ", studentAnswers.Select(s => s.Id))
					));
					throw e;
				}
				// It is necessary to interact with the database inside the for loop because we want
				// to update the sent objects exactly after they are sent (in case next loop 
				// iteration's requests fail).
				studentAnswers.ForEach(s => s.IsSentToTkReports = true);
				await _studentAnswers.Update(studentAnswers);
			}
		}

		private async Task LogInToTkReports(string token) {
			var tokenDTO = new TkReportLoginDTO(){ token = token };
			var response = await _form.Post<TkReportLoginResponseDTO>(loginPath, tokenDTO);
			CheckRequestCode(response.code);
			_form.SetAuth(response.JWT);
		}

		private async Task SendEmatAnswers(List<StudentAnswer> studentAnswers, List<Activity> activities)
		{
			var studentAnswerDTOs = studentAnswers.ConvertAll<EmatAnswerTkReportsDTO>(s => {
				var activityId = GetActivityIdFromStudentAnswer(s, activities);
				return new EmatAnswerTkReportsDTO(s, EmatActivityViewerUrl + activityId);
			});
			var parsedResponse = JsonConvert.SerializeObject(new { exercises = studentAnswerDTOs });
			var response = await _form.Post<PostResponseDTO>(ematPath, new { data = parsedResponse });
			CheckRequestCode(response.code);
		}

		private async Task SendLudiAnswers(List<StudentAnswer> studentAnswers, List<Activity> activities)
		{
			var studentAnswerDTOs = studentAnswers.ConvertAll<AnswerTkReportsDTO>(s => {
				var activityId = GetActivityIdFromStudentAnswer(s, activities);
				return new AnswerTkReportsDTO(s, LudiActivityViewerUrl + activityId);
			});
			var parsedResponse = JsonConvert.SerializeObject(new { exercises = studentAnswerDTOs });
			var response = await _form.Post<PostResponseDTO>(ludiPath, new { data = parsedResponse });
			CheckRequestCode(response.code);
		}

		private void CheckRequestCode(string code)
		{
			if (code != "200") throw new TkReportsException();
		}
		
		private Guid GetActivityIdFromStudentAnswer(StudentAnswer studentAnswer, List<Activity> activities)
		{
			return activities.Where(a =>
					a.Course.Number == studentAnswer.Course
					&& a.Session == studentAnswer.ActivitySession
					&& a.ContentBlockId == studentAnswer.ActivityContentBlockId
					&& a.Difficulty == studentAnswer.ActivityDifficulty
				)
				.Select(a => a.Id)
				.FirstOrDefault();
		}
		
		#region DTODefinition
		
		// Defines a login to TkReports. Token is the token provided to connect to Tkreports.
		private class SubjectLanguageRequest 
		{
			public string Subject { get; set; }
			public string Language { get; set; }
			public string Token { get; set; }
		}

		// Defines the object sent to the login endpoint
		private class TkReportLoginDTO
		{
			public string token { get; set; }
		}

		// Defines the base object received on every request
		private class PostResponseDTO
		{
			public string code { get; set; }
		}

		// Defines the object received on successful login
		private class TkReportLoginResponseDTO: PostResponseDTO
		{
			public string JWT { get; set; }
		}

		// Defines the object sent to studenAnswers-Ludi endpoint
		private class AnswerTkReportsDTO
		{
			public string studentId { get; set; }
			public string sessionNumber { get; set; }
			public string courseCode { get; set; }
			public DateTime registeredAt { get; set; }
			public string level { get; set; }
			public float score { get; set; }
			public string activityUrl { get; set; }

			public AnswerTkReportsDTO(StudentAnswer s, string url)
			{
				studentId = s.UserName;
				sessionNumber = s.Session.ToString();
				courseCode = s.Course.ToString();
				registeredAt = s.CreatedAt;
				level = ToLevelName(s.Level);
				score = s.Grade * 10;
				activityUrl = url;
			}

			// Transforming the level to the strings the TkReports api accepts.
			private string ToLevelName(LevelType l) {
				switch (l)
				{
					case LevelType.Advance: return "avanza";
					case LevelType.Practise: return "practica";
					case LevelType.Revise: return "repasa";
				}
				return "repasa2";
			}
		}

		// Defines the object sent to studentAnswers-Emat endpoint
		private class EmatAnswerTkReportsDTO: AnswerTkReportsDTO 
		{
			public int contentBlockId { get; set; }
			public int passed { get; set; }

			public EmatAnswerTkReportsDTO(StudentAnswer s, string url): base(s, url)
			{
				contentBlockId = s.Stage;
				passed = s.Grade >= Config.PassGrade ? 1 : 0;
			}
		}

		// Exception thrown when a request to tkreports fails
		private class TkReportsException: Exception {}

		#endregion
	}
}