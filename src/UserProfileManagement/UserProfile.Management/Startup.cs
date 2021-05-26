using System;
using System.Linq;
using FluentValidation.AspNetCore;
using Hive.Common.Core;
using Hive.Common.Core.Filters;
using Hive.Common.Core.Identity;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Security.Handlers;
using Hive.Common.Core.Security.Requirements;
using Hive.Common.Core.Services;
using Hive.UserProfile.Application;
using Hive.UserProfile.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace UserProfile.Management
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
        public void ConfigureServices(IServiceCollection services)
        {
            var authority = Configuration.GetValue<string>("Authority");
            
            services.AddUserProfileCore();
            services.AddUserProfileInfrastructure(Configuration);
            
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
                    policy.AddRequirements(new OnlyOwnerAuthorizationRequirement(Array.Empty<string>())));
            });

            services.AddCors(options =>
            {
                options.AddPolicy(name: "Angular",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });
            
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IDateTimeService, DateTimeService>();
            services.AddSingleton<IAuthorizationHandler, EntityOwnerAuthorizationHandler>();
            
            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "User Profile Management API";
                configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });

                configure.OperationProcessors.Add(new AspNetCoreOperationSecurityScopeProcessor("JWT"));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwaggerUi3(settings =>
            {
                settings.Path = "/api";
                settings.DocumentPath = "/api/specification.json";
            });

            app.UseHttpsRedirection();
            
            app.UseRouting();
            
            app.UseCors("Angular");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}