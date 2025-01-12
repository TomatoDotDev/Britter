using Britter.DataAccess.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Britter.DataAccess.Seeder
{
    [ExcludeFromCodeCoverage(Justification = "Seeder method")]
    public class SeedData
    {
        public static async Task SeedAdminAccountAsync(IServiceProvider serviceProvider)
        {
            var adminEmail = "administrator@britter.com";
            var _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();
            if (_roleManager != null)
            {
                IdentityRole<Guid>? role = await _roleManager.FindByNameAsync("admin");
                if (role == null)
                {
                    var results = await _roleManager.CreateAsync(new IdentityRole<Guid>("admin"));
                    role = await _roleManager.FindByNameAsync("admin");
                }

                var userManager = serviceProvider.GetRequiredService<UserManager<BritterUser>>();
                if (userManager != null)
                {
                    BritterUser? admin = await userManager.FindByNameAsync(adminEmail);
                    if (admin == null)
                    {
                        var results = await userManager.CreateAsync(new BritterUser()
                        {
                            UserName = adminEmail,
                            Email = adminEmail,

                        }, "Password123!");

                        admin = await userManager.FindByNameAsync(adminEmail);
                    }

                    var result = await userManager.AddToRoleAsync(admin!, role!.Name!);
                }
            }
        }
    }
}
