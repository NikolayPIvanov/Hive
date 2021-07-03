using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hive.Identity.Contracts;
using Hive.Identity.Models;
using Hive.Identity.Services;
using Microsoft.AspNetCore.Identity;
using Serilog;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Hive.Identity.Data
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, 
                                                      RoleManager<IdentityRole> roleManager,
                                                      ApplicationDbContext context, IIdentityDispatcher dispatcher, IRedisCacheClient cacheClient)
        {
            await SeedOfType(IdentityType.Admin, "admin@gmail.com", "Admin", "One", userManager, roleManager, context, dispatcher, cacheClient);
            await SeedOfType(IdentityType.Buyer, "buyer@gmail.com", "Buyer", "One", userManager, roleManager, context, dispatcher,cacheClient);
            await SeedOfType(IdentityType.Seller, "seller@gmail.com", "Seller", "One", userManager, roleManager, context, dispatcher,cacheClient);
            await SeedOfType(IdentityType.Investor, "investor@gmail.com", "Investor", "One",  userManager, roleManager, context, dispatcher,cacheClient);
        }
        
        private static async Task SeedOfType(
            IdentityType type, string email, 
            string givenName, string surname,
            UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, 
            ApplicationDbContext context, IIdentityDispatcher dispatcher, IRedisCacheClient cacheClient)
        {
            const string defaultPassword = "YourStr0ngPassword!";

            var userRoles = new List<string>();
            var userTypes = new[] {type};
            foreach (var role in userTypes.Select(userType => new IdentityRole(userType.ToString())))
            {
                userRoles.Add(role.Name);
                if (roleManager.Roles.All(r => r.Name != role.Name))
                {
                    await roleManager.CreateAsync(role);
                }
            }

            var user = new ApplicationUser() { UserName = email, Email = email, EmailConfirmed = true };
            if (userManager.Users.All(u => u.UserName != user.UserName))
            {
                await userManager.CreateAsync(user, defaultPassword);
                await userManager.AddToRolesAsync(user, userRoles);
                
                await StoreUserNameInCache(cacheClient, user);
                await DispatchUserCreatedEvents(dispatcher, user.Id,  givenName, surname, userTypes.ToList());
            }
            
            await context.SaveChangesAsync();
        }

        public static async Task StoreUserNameInCache(IRedisCacheClient cacheClient, ApplicationUser user)
        {
            var key = $"usernames:{user.Id}";
            _ = await cacheClient.GetDbFromConfiguration().AddAsync(key, user.UserName);
        }

        public static async Task DispatchUserCreatedEvents(IIdentityDispatcher dispatcher, 
            string userId, string givenName, string surname, List<IdentityType> userTypes)
        {
            Log.Logger.Warning($"Before publishing for {userId}");
            await dispatcher.PublishUserCreatedEventAsync(userId, givenName, surname);

            foreach (var type in userTypes)
            {
                await dispatcher.PublishUserTypeEventAsync(userId, type);
            }
        }
    }
}