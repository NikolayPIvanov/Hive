using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using BuildingBlocks.Core.Interfaces;
using Hive.Identity.Contracts;
using Hive.Identity.Data;
using Hive.Identity.Models;
using Hive.Identity.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Logging;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Hive.Identity.Areas.Identity.Pages.Account
{
    // quick workaround
    public enum InputUserType
    {
        Buyer = 0,
        Seller,
        Investor
    }
    
    [AllowAnonymous]
    public class RegisterModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IRedisCacheClient _cacheClient;
        private readonly IIdentityDispatcher _dispatcher;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailService _emailService;

        public RegisterModel(
            IIdentityDispatcher dispatcher,
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IRedisCacheClient cacheClient,
            ILogger<RegisterModel> logger,
            IEmailService emailService)
        {
            _dispatcher = dispatcher;
            _userManager = userManager;
            _signInManager = signInManager;
            _cacheClient = cacheClient;
            _logger = logger;
            _emailService = emailService;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public IList<AuthenticationScheme> ExternalLogins { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            
            [Required]
            [Display(Name = "Given Name")]
            public string GivenName { get; set; }
            
            [Required]
            [Display(Name = "Surname")]
            public string Surname { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }

            [Required]
            [EnumDataType(typeof(InputUserType))]
            public InputUserType AccountType { get; set; }
        }

        
        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser
                {
                    UserName = Input.Email, 
                    Email = Input.Email
                };
                var result = await _userManager.CreateAsync(user, Input.Password);
                
                
                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");
                    var roleOp = await _userManager.AddToRoleAsync(user, Input.AccountType.ToString());

                    if (roleOp.Succeeded)
                    {
                        var accountType = (IdentityType)Enum.Parse(typeof(IdentityType), Input.AccountType.ToString());
                        await ApplicationDbContextSeed.DispatchUserCreatedEvents(
                            _dispatcher, user.Id, Input.GivenName, Input.Surname,
                            new List<IdentityType>() {accountType});
                        await ApplicationDbContextSeed.StoreUserNameInCache(_cacheClient, user);
                    }

                    await SendEmailVerification(user, returnUrl);
                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private async Task SendEmailVerification(ApplicationUser user, string returnUrl)
        {
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            var callbackUrl = Url.Page(
                "/Account/ConfirmEmail",
                pageHandler: null,
                values: new { area = "Identity", userId = user.Id, code = code, returnUrl = returnUrl },
                protocol: Request.Scheme);

            await _emailService.SendAsync(
                new []{ Input.Email },
                "Confirm your email",
                $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");
        }
    }
}
