// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.

using System.Reflection;
using BuildingBlocks.Core.Caching;
using BuildingBlocks.Core.Email;
using BuildingBlocks.Core.MessageBus;
using DotNetCore.CAP;
using Duende.IdentityServer;
using Hive.Common.Core;
using Hive.Identity.Data;
using Hive.Identity.Jobs;
using Hive.Identity.Models;
using Hive.Identity.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

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

            var issuerUri = Configuration.GetValue<string>("IssuerUri");
            services.AddIdentityServer(options =>
                {
                    options.IssuerUri = issuerUri;
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

            services.AddAuthentication();

            services.AddHostedService<RoleSeeder>();
        }

        public void Configure(IApplicationBuilder app)
        {
            if (Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
            });

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
    }
}