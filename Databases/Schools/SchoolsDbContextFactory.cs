using System;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Api.Databases.Schools
{
    public class SchoolsDbContextFactory : IDesignTimeDbContextFactory<SchoolsDbContext>
    {
        public SchoolsDbContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var builder = new DbContextOptionsBuilder<SchoolsDbContext>();
            var connectionString = configuration.GetConnectionString("SchoolsConnection");
            builder.UseSqlServer(connectionString,
                options => options.CommandTimeout((int)TimeSpan.FromMinutes(2).TotalSeconds));

            return new SchoolsDbContext(builder.Options);
        }
    }
}