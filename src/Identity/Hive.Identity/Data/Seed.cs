using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hive.Identity.Models;
using Hive.Identity.Services;
using Microsoft.AspNetCore.Identity;

namespace Hive.Identity.Data
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, 
                                                      ApplicationDbContext context, IIdentityDispatcher dispatcher)
        {
            await SeedOfType(new [] {AccountType.Admin, AccountType.Seller, AccountType.Investor}, "admin@gmail.com", userManager, roleManager, context, dispatcher);
            await SeedOfType(new [] {AccountType.Buyer}, "buyer@gmail.com", userManager, roleManager, context, dispatcher);
            await SeedOfType(new [] {AccountType.Seller}, "seller@gmail.com", userManager, roleManager, context, dispatcher);
            await SeedOfType(new [] {AccountType.Investor}, "investor@gmail.com", userManager, roleManager, context, dispatcher);
        }
        
        private static async Task SeedOfType(IEnumerable<AccountType> types, string email, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, ApplicationDbContext context, IIdentityDispatcher dispatcher)
        {
            const string defaultPassword = "YourStr0ngPassword!";

            var userRoles = new List<string>();
            var userTypes = types.ToList();
            foreach (var role in userTypes.Select(userType => new IdentityRole(userType.ToString())))
            {
                userRoles.Add(role.Name);
                if (roleManager.Roles.All(r => r.Name != role.Name))
                {
                    await roleManager.CreateAsync(role);
                }
            }

            var user = new ApplicationUser(userTypes) { UserName = email, Email = email };
            if (userManager.Users.All(u => u.UserName != user.UserName))
            {
                await userManager.CreateAsync(user, defaultPassword);
                await userManager.AddToRolesAsync(user, userRoles);
            }
            
            await context.SaveChangesAsync();

            foreach (var type in userTypes)
            {
                await dispatcher.PublishUserCreatedEventAsync(user.Id, type);
            }
        }
    }
}