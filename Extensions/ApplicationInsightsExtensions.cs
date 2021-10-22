using Api.Interfaces.Shared;
using Microsoft.ApplicationInsights.Channel;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Api.Extensions
{
    public static class ApplicationInsightsExtensions
    {
        public static IServiceCollection AddApplicationInsights(this IServiceCollection services)
        {
            services.AddApplicationInsightsTelemetry();
            services.AddSingleton<ITelemetryInitializer, CustomTelemetryInitializer>();
            services.AddApplicationInsightsTelemetryProcessor<SubjectTelemetryProcessor>();

            return services;
        }

        private class CustomTelemetryInitializer : ITelemetryInitializer
        {
            private const string RoleName = "Cibers API";
            public void Initialize(ITelemetry telemetry)
            {
                telemetry.Context.Cloud.RoleName = RoleName;
            }
        }

        private class SubjectTelemetryProcessor : ITelemetryProcessor
        {
            private const string SubjectTagKey = "subject";
            private readonly IHttpContextAccessor _httpContext;
            private readonly IHttpContextService _httpContextExtensions;
            private readonly ITelemetryProcessor _next;

            public SubjectTelemetryProcessor(
                IHttpContextAccessor httpContext,
                IHttpContextService httpContextExtensions,
                ITelemetryProcessor next)
            {
                _httpContext = httpContext;
                _httpContextExtensions = httpContextExtensions;
                _next = next;
            }

            public void Process(ITelemetry item)
            {
                if (_httpContext.HttpContext?.Request != null)
                {
                    item.Context.GlobalProperties[SubjectTagKey] = _httpContextExtensions.GetSubjectFromUri();
                }
                _next.Process(item);
            }
        }
    }
}
