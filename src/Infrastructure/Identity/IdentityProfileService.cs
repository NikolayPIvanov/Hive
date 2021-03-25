// using System;
// using System.Linq;
// using System.Security.Claims;
// using System.Threading.Tasks;
// using Hive.Infrastructure.Persistence;
// using IdentityServer4.Extensions;
// using IdentityServer4.Models;
// using IdentityServer4.Services;
// using Microsoft.AspNetCore.Identity;
// using Microsoft.EntityFrameworkCore;
//
// namespace Hive.Infrastructure.Identity
// {
//     public class IdentityProfileService : IProfileService
//     {
//         private readonly IUserClaimsPrincipalFactory<ApplicationUser> _claimsFactory;
//         private readonly UserManager<ApplicationUser> _userManager;
//         private readonly ApplicationDbContext _context;
//
//         public IdentityProfileService(IUserClaimsPrincipalFactory<ApplicationUser> claimsFactory, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
//         {
//             _claimsFactory = claimsFactory;
//             _userManager = userManager;
//             _context = context;
//         }
//         
//         public async Task GetProfileDataAsync(ProfileDataRequestContext context)
//         {
//             var sub = context.Subject.GetSubjectId();
//             var user = await _userManager.FindByIdAsync(sub);
//             if (user == null)
//             {
//                 throw new ArgumentException("");
//             }
//
//             var principal = await _claimsFactory.CreateAsync(user);
//             var claims = principal.Claims.ToList();
//
//             var sellerAccount = string.Empty;
//             if (sellerAccount != null)
//             {
//                 claims.Add(new Claim("seller_acc", sellerAccount));
//             }
//             
//             context.IssuedClaims = claims;
//         }
//
//         public async Task IsActiveAsync(IsActiveContext context)
//         {
//             var sub = context.Subject.GetSubjectId();
//             var user = await _userManager.FindByIdAsync(sub);
//             context.IsActive = user != null;
//         }
//     }
// }