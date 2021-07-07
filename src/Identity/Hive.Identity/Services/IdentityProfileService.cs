using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Duende.IdentityServer.Extensions;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using Hive.Identity.Data;
using Hive.Identity.Models;
using Microsoft.AspNetCore.Identity;

namespace Hive.Identity.Services
{
    public class IdentityProfileService : IProfileService
    {
        private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        private const string externalAccountId = "externalAccountId";

        public IdentityProfileService(IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, UserManager<ApplicationUser> userManager, 
                                      RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _claimsFactory = claimsFactory;
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
        }
        
        public async Task GetProfileDataAsync(ProfileDataRequestContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            if (user == null)
            {
                throw new ArgumentException("User was not found.");
            }
            var principal = await _claimsFactory.CreateAsync(user);
            var claims = principal.Claims.ToList();

            var roles = await _userManager.GetRolesAsync(user);
            foreach (var role in roles)
            {
                claims.Add(new Claim(ClaimsIdentity.DefaultRoleClaimType, role));
            }

            if (user.ExternalAccountId.HasValue)
            {
                claims.Add(new Claim(externalAccountId, user.ExternalAccountId.Value.ToString()));
            }
            
            context.IssuedClaims = claims;
        }

        public async Task IsActiveAsync(IsActiveContext context)
        {
            var sub = context.Subject.GetSubjectId();
            var user = await _userManager.FindByIdAsync(sub);
            context.IsActive = user != null;
        }
    }
}