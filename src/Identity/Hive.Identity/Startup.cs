// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using System;
using System.Reflection;
using BuildingBlocks.Core.Caching;
using BuildingBlocks.Core.Email;
using BuildingBlocks.Core.MessageBus;
using DotNetCore.CAP;
using Duende.IdentityServer;
using Hive.Common.Core;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Services;
using Hive.Identity.Data;
using Hive.Identity.Models;
using Hive.Identity.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using StackExchange.Redis.Extensions.Core.Abstractions;

namespace Hive.Identity
{
    public class Startup
    {
        public IWebHostEnvironment Environment { get; }
        public IConfiguration Configuration { get; }

        public Startup(IWebHostEnvironment environment, IConfiguration configuration)
        {
            Environment = environment;
            Configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");
            var assembly = typeof(Startup).Assembly.FullName;
            
            services.AddControllersWithViews();
            services.AddRazorPages();
            
            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString,
                    o => o.MigrationsAssembly(assembly)));
            
            services.AddSendGrid(Configuration);
            services.AddRedis(Configuration);
            services.AddRabbitMqBroker<ApplicationDbContext>(false, connectionString, Configuration);
            services.AddOfType<ICapSubscribe>(new []{ Assembly.GetExecutingAssembly() });
            services.AddScoped<IIdentityDispatcher, IdentityDispatcher>();

            services.AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    options.SignIn.RequireConfirmedAccount = true;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders()
                .AddDefaultUI();

            services.AddIdentityServer(options =>
                {
                    options.Events.RaiseErrorEvents = true;
                    options.Events.RaiseInformationEvents = true;
                    options.Events.RaiseFailureEvents = true;
                    options.Events.RaiseSuccessEvents = true;

                    // see https://docs.duendesoftware.com/identityserver/v5/fundamentals/resources/
                    options.EmitStaticAudienceClaim = true;
                })
                .AddConfigurationStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(assembly));
                })
                .AddOperationalStore(options =>
                {
                    options.ConfigureDbContext = b => b.UseSqlServer(connectionString,
                        sql => sql.MigrationsAssembly(assembly));
                })
                .AddAspNetIdentity<ApplicationUser>()
                .AddProfileService<IdentityProfileService>();

            services.AddAuthentication()
                .AddGoogle(options =>
                {
                    options.SignInScheme = IdentityServerConstants.ExternalCookieAuthenticationScheme;

                    // register your IdentityServer with Google at https://console.developers.google.com
                    // enable the Google+ API
                    // set the redirect URI to https://localhost:5001/signin-google
                    options.ClientId = "copy client ID from Google here";
                    options.ClientSecret = "copy client secret from Google here";
                });
        }

        public void Configure(IApplicationBuilder app)
        {
            SeedDefaultData(app);

            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseStaticFiles();

            app.UseRouting();
            app.UseIdentityServer();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }

        private static void SeedDefaultData(IApplicationBuilder app)
        {
            using var serviceScope = app.ApplicationServices.GetService<IServiceScopeFactory>()?.CreateScope();
            if (serviceScope == null)
                throw new Exception("Cannot create IServiceScopeFactory");
            var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var dispatcher = serviceScope.ServiceProvider.GetRequiredService<IIdentityDispatcher>();
            var context = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var cache = serviceScope.ServiceProvider.GetRequiredService<IRedisCacheClient>();

            ApplicationDbContextSeed.SeedDefaultUserAsync(userManager, roleManager, context, dispatcher, cache).Wait();
        }
    }
}