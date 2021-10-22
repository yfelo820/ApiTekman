using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Api.Databases.Identity
{
    public class ApiIdentityDbContextFactory : IDesignTimeDbContextFactory<ApiIdentityDbContext>
    {
        public ApiIdentityDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<ApiIdentityDbContext>();
            var connectionString = configuration.GetConnectionString("IdentityConnection");
            builder.UseSqlServer(connectionString);

            return new ApiIdentityDbContext(builder.Options);
        }
    }
}