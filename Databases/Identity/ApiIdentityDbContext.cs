using Api.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;


namespace Api.Databases.Identity
{
    public class ApiIdentityDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApiIdentityDbContext(DbContextOptions<ApiIdentityDbContext> options)
            : base(options)
        {
        }

        public DbSet<UniversalUserProperties> UniversalUserProperties { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UniversalUserProperties>().HasKey(usp => usp.UserId);
            builder.Entity<UniversalUserProperties>().Property(usp => usp.Name).IsRequired();

            builder.Entity<ApplicationUser>()
                .HasOne(u => u.UniversalUserProperties)
                .WithOne(c => c.User)
                .HasForeignKey<UniversalUserProperties>(usp => usp.UserId);
        }
    }
}