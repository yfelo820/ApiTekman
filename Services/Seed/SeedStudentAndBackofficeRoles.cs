using System.Linq;
using System.Threading.Tasks;
using Api.Constants;
using Api.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Api.Services.Seed
{
    public static class SeedUserRoles
	{
		public static async Task Seed(
			UserManager<ApplicationUser> userManager, 
			RoleManager<IdentityRole> roleManager)
		{
			if (!(await roleManager.RoleExistsAsync(Role.Student))) {
				await roleManager.CreateAsync(new IdentityRole(Role.Student));
				await roleManager.CreateAsync(new IdentityRole(Role.Backoffice));

				foreach (var user in userManager.Users.ToList())
				{
					await userManager.AddToRoleAsync(user, Role.Backoffice);
				}
			}
		}
	}
}