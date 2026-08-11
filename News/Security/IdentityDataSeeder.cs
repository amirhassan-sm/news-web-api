using Infrastructure.Security.Idetity.Models;
using Microsoft.AspNetCore.Identity;

namespace News.Security
{
    public static class AppRoles
    {
        public const string Admin = "admin";
        public const string User = "user";
    }
    public static class IdentityDataSeeder
    {
        public static async Task SeedAsync(IServiceProvider service)
        {
            using var scope = service.CreateScope();
            var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<ApplicationRole>>();
            foreach (var roleName in new[] { AppRoles.Admin, AppRoles.User })
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new ApplicationRole
                    {
                        Name = roleName,
                        
                    });
                }
            }

        }
    }
}
