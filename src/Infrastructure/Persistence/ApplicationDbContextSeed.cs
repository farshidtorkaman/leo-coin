using System.Collections.Generic;
using Crypto.Domain.Entities;
using Crypto.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Crypto.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultRoleAsync(RoleManager<IdentityRole> roleManager)
        {
            if (!roleManager.Roles.Any())
            {
                await roleManager.CreateAsync(new IdentityRole("admin"));
                await roleManager.CreateAsync(new IdentityRole("user"));
            }
        }

        // public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager)
        // {
        //     var defaultUser = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };
        //
        //     if (userManager.Users.All(u => u.UserName != defaultUser.UserName))
        //     {
        //         await userManager.CreateAsync(defaultUser, "Administrator1!");
        //     }
        // }

        public static async Task SeedSampleDataAsync(ApplicationDbContext context)
        {
            // Seed, if necessary
        }
    }
}
