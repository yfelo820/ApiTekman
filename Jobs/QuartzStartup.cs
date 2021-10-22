using System;
using System.Collections.Specialized;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Impl;
using Quartz.Spi;

namespace Api.Jobs
{
	// Quartz and job setup: 
	// https://www.quartz-scheduler.net/documentation/quartz-3.x/tutorial/using-quartz.html
	public static class QuartzStartup
	{
		public static async void StartJobs(IServiceCollection services, IConfiguration configuration)
		{
			var intervalInMinutes = Int32.Parse(configuration["TkReports:minuteInterval"]);
			if (intervalInMinutes <= 0) return;

			NameValueCollection props = new NameValueCollection
			{
				{ "quartz.serializer.type", "binary" }
			};
			StdSchedulerFactory factory = new StdSchedulerFactory(props);
			
			// Registering jobs in the DI
			services.AddScoped<StudentAnswersToTkReportsJob>();
			
			// Creating scheduler
			IScheduler scheduler = await factory.GetScheduler();
			scheduler.JobFactory = new JobFactory(services.BuildServiceProvider());
			await scheduler.Start();

			// Registering jobs to quartz
			IJobDetail job = JobBuilder.Create<StudentAnswersToTkReportsJob>()
				.WithIdentity("sendStudentAnswersToTkReports", "tkReports")
				.Build();
			ITrigger trigger = TriggerBuilder.Create()
				.WithIdentity("triggerTkReports", "tkReports")
				.StartNow()
				.WithSimpleSchedule(x => x
					.WithIntervalInMinutes(intervalInMinutes)
					.RepeatForever()
				)
				.Build();
			
			await scheduler.ScheduleJob(job, trigger);
		}

		// Class for creating jobs with dependency injection:
		// https://stackoverflow.com/questions/42157775
		private class JobFactory : IJobFactory
		{
			protected readonly IServiceProvider Container;

			public JobFactory(IServiceProvider container) => Container = container;

			public IJob NewJob(TriggerFiredBundle bundle, IScheduler scheduler)
			{
				return Container.GetService(bundle.JobDetail.JobType) as IJob;
			}

			public void ReturnJob(IJob job) {}
		}
	}
}