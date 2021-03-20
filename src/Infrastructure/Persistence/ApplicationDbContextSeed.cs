using Hive.Domain.Entities;
using Hive.Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Threading.Tasks;

namespace Hive.Infrastructure.Persistence
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            var administratorRole = new IdentityRole("Administrator");

            if (roleManager.Roles.All(r => r.Name != administratorRole.Name))
            {
                await roleManager.CreateAsync(administratorRole);
            }

            var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

            if (userManager.Users.All(u => u.UserName != administrator.UserName))
            {
                await userManager.CreateAsync(administrator, "Administrator1!");
                await userManager.AddToRolesAsync(administrator, new [] { administratorRole.Name });
            }
        }
        
        public static async Task Seed(ApplicationDbContext context)
        {
            FakeData.Init(10);
            await SeedCategories(context);
        }

        private static Task SeedCategories(ApplicationDbContext context)
        {
            if (!context.Categories.Any())
            {
                context.AddRange(FakeData.Categories);
                return context.SaveChangesAsync();
            }

            return Task.CompletedTask;
        }
    }
}
