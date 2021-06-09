using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hive.Identity.Contracts;
using Hive.Identity.Models;
using Hive.Identity.Services;
using Microsoft.AspNetCore.Identity;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Hive.Identity.Data
{
    public static class ApplicationDbContextSeed
    {
        public static async Task SeedDefaultUserAsync(UserManager<ApplicationUser> userManager, 
                                                      RoleManager<IdentityRole> roleManager,
                                                      ApplicationDbContext context, IIdentityDispatcher dispatcher, IRedisCacheClient cacheClient)
        {
            await SeedOfType(new [] {IdentityType.Admin, IdentityType.Seller, IdentityType.Investor, IdentityType.Buyer}, "admin@gmail.com", userManager, roleManager, context, dispatcher, cacheClient);
            await SeedOfType(new [] {IdentityType.Buyer}, "buyer@gmail.com", userManager, roleManager, context, dispatcher,cacheClient);
            await SeedOfType(new [] {IdentityType.Seller}, "seller@gmail.com", userManager, roleManager, context, dispatcher,cacheClient);
            await SeedOfType(new [] {IdentityType.Investor}, "investor@gmail.com", userManager, roleManager, context, dispatcher,cacheClient);
        }
        
        private static async Task SeedOfType(IEnumerable<IdentityType> types, string email, UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager, ApplicationDbContext context, IIdentityDispatcher dispatcher, IRedisCacheClient cacheClient)
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
                
                await StoreInCache(cacheClient, user);

                await DispatchEvents(dispatcher, user, userTypes);
            }
            
            await context.SaveChangesAsync();
        }

        private static async Task StoreInCache(IRedisCacheClient cacheClient, ApplicationUser user)
        {
            var key = $"username:{user.Id}";
            _ = await cacheClient.GetDbFromConfiguration().AddAsync(key, user.UserName);
        }

        private static async Task DispatchEvents(IIdentityDispatcher dispatcher, ApplicationUser user, List<IdentityType> userTypes)
        {
            await dispatcher.PublishUserCreatedEventAsync(user.Id);

            foreach (var type in userTypes)
            {
                await dispatcher.PublishUserTypeEventAsync(user.Id, type);
            }
        }
    }
}