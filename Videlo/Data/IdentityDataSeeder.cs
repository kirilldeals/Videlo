using GamevaWeb.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Videlo.Configuration;
using Videlo.Models.Database;

namespace Videlo.Data
{
    public class IdentityDataSeeder
    {
        public static async Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var userManager = serviceProvider.GetRequiredService<UserManager<User>>();
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var adminCredentials = serviceProvider.GetRequiredService<IOptions<AdminCredentialsSettings>>().Value;

            var roleNames = new[] { RoleConstants.Admin, RoleConstants.Moderator, RoleConstants.User };
            foreach (var roleName in roleNames)
            {
                if (await roleManager.FindByNameAsync(roleName) == null)
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }

            var userName = adminCredentials.Username;
            var email = adminCredentials.Email;
            var password = adminCredentials.Password;

            if (await userManager.FindByNameAsync(userName) == null)
            {
                User admin = new() { Email = email, UserName = userName, CreatedAt = DateTime.UtcNow };
                IdentityResult result = await userManager.CreateAsync(admin, password);
                if (result.Succeeded)
                {
                    await userManager.AddToRoleAsync(admin, RoleConstants.Admin);
                }
            }
        }
    }
}
