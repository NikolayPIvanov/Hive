using FluentValidation.AspNetCore;
using Hive.Common.Core;
using Hive.Common.Core.Filters;
using Hive.Common.Core.Identity;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Security.Requirements;
using Hive.Common.Core.Services;
using Hive.Gig.Application;
using Hive.Gig.Infrastructure;
using Hive.LooselyCoupled.Authorization.Requirements;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Gig.Management
{
    public class Startup
    {
        private const string DefaultAuthenticationSchema = "Bearer";
        
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }
        
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            var authority = Configuration.GetValue<string>("Authority");
            
            services.AddGigsCore();
            services.AddGigsInfrastructure(Configuration);
            
            services.AddHttpContextAccessor();
            
            services.AddControllers(options =>
            {
                options.AllowEmptyInputInBodyModelBinding = true;
                options.Filters.Add<ApiExceptionFilterAttribute>();
            }).AddFluentValidation();
            
            services.AddAuthentication(DefaultAuthenticationSchema)
                .AddJwtBearer(DefaultAuthenticationSchema, options =>
                {
                    options.Authority = authority;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateAudience = false
                    };
                });
            
            services.AddAuthorization(options =>
            {
                options.AddPolicy("OnlyOwnerPolicy", policy =>
                    policy.AddRequirements(new OnlyOwnerAuthorizationRequirement()));
            });
            
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IDateTimeService, DateTimeService>();
            
            services.AddSingleton<IAuthorizationHandler, EntityOwnerAuthorizationHandler>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}