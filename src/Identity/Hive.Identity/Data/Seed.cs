using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hive.Identity.Contracts;
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
            await SeedOfType(new [] {IdentityType.Admin, IdentityType.Seller, IdentityType.Investor}, "admin@gmail.com", userManager, roleManager, context, dispatcher);
            await SeedOfType(new [] {IdentityType.Buyer}, "buyer@gmail.com", userManager, roleManager, context, dispatcher);
            await SeedOfType(new [] {IdentityType.Seller}, "seller@gmail.com", userManager, roleManager, context, dispatcher);
            await SeedOfType(new [] {IdentityType.Investor}, "investor@gmail.com", userManager, roleManager, context, dispatcher);
        }
        
        private static async Task SeedOfType(IEnumerable<IdentityType> types, string email, UserManager<ApplicationUser> userManager,
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

            var user = new ApplicationUser(userTypes) { UserName = email, Email = email, EmailConfirmed = true };
            if (userManager.Users.All(u => u.UserName != user.UserName))
            {
                await userManager.CreateAsync(user, defaultPassword);
                await userManager.AddToRolesAsync(user, userRoles);
                
                await dispatcher.PublishUserCreatedEventAsync(user.Id);

                foreach (var type in userTypes)
                {
                    await dispatcher.PublishUserTypeEventAsync(user.Id, type);
                }
            }
            
            await context.SaveChangesAsync();

            
        }
    }
}