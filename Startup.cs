using System.IdentityModel.Tokens.Jwt;
using System.Reflection;
using System.Threading.Tasks;
using Api.Databases.Content;
using Api.Databases.Identity;
using Api.Databases.Schools;
using Api.Extensions;
using Api.Filters;
using Api.Identity.Models;
using Api.Interfaces.Shared;
using Api.Jobs;
using Api.Services.Seed;
using Api.SignalR;
using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Api
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IHostingEnvironment Environment { get; }

        public Startup(IConfiguration configuration, IHostingEnvironment environment)
        {
            Configuration = configuration;
            Environment = environment;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton(Configuration);
            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddApplicationInsights();
            services.AddSignalR(); 
            RegisterDbContext(services);
            services.ConfigureContainer(Configuration);
            RegisterMvcIdentityServices(services);
            services.RegisterEmailService(Configuration);
            services.AddAutoMapper();
            QuartzStartup.StartJobs(services, Configuration);
            services.AddMediatR(Assembly.GetExecutingAssembly());
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                //InitializeDatabase(app).GetAwaiter().GetResult();
            }
            app.UseAuthentication();
            app.UseMvc();
            app.UseSignalR(endpoints =>
            {
                endpoints.MapHub<LiveStudentHub>("/livestudents");
            });
            
        }

        #region Database Initialization

        private async Task InitializeDatabase(IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope())
            {
                scope.ServiceProvider.GetRequiredService<ContentDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<ApiIdentityDbContext>().Database.Migrate();
                scope.ServiceProvider.GetRequiredService<SchoolsDbContext>().Database.Migrate();

                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                await SeedUserRoles.Seed(userManager, roleManager);
            }
        }

        #endregion

        #region Service registration

        private void RegisterDbContext(IServiceCollection services)
        {
            services.AddDbContext<ContentDbContext>(c =>
            {
                c.UseSqlServer(Configuration.GetConnectionString("ContentConnection"));
            });
            services.AddDbContext<ApiIdentityDbContext>(c =>
            {
                c.UseSqlServer(Configuration.GetConnectionString("IdentityConnection"));
            });
            services.AddDbContext<SchoolsDbContext>(c =>
            {
                c.UseSqlServer(Configuration.GetConnectionString("SchoolsConnection"));
            });
            services.AddScoped<IUnitOfWork, SchoolsUnitOfWork>();
        }

        private void RegisterMvcIdentityServices(IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireDigit = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddEntityFrameworkStores<ApiIdentityDbContext>()
            .AddDefaultTokenProviders();

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.ConfigureAuthentication(Configuration, Environment);
            services.AddMvc(config =>
            {
                config.Filters.Add(typeof(HttpExceptionFilter));
            });
            services.AddAuthorizationPolicies(Configuration);

        }
        #endregion
    }
}