using System;
using System.Linq;
using FluentValidation.AspNetCore;
using Hive.Common.Core.Filters;
using Hive.Common.Core.Identity;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Security.Handlers;
using Hive.Common.Core.Security.Requirements;
using Hive.Common.Core.Services;
using Hive.Investing.Application;
using Hive.Investing.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using NSwag;
using NSwag.Generation.Processors.Security;

namespace Investing.Management
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
            
            services.AddInvestingApplication();
            services.AddInvestingInfrastructure(Configuration);
            
            services.AddHttpContextAccessor();
            
            services.AddControllers(options =>
            {
                options.AllowEmptyInputInBodyModelBinding = true;
                options.Filters.Add<ApiExceptionFilterAttribute>();
            }).AddFluentValidation();
            
            services.AddCors(options =>
            {
                var origins = Configuration.GetSection("CorsOrigins").Get<string[]>();
                options.AddDefaultPolicy(
                    builder =>
                    {
                        builder
                            .WithOrigins(origins)
                            .AllowCredentials()
                            .AllowAnyHeader()
                            .SetIsOriginAllowed(_ => true)
                            .AllowAnyMethod();
                    });
            });

            
            
            
            services.AddAuthentication(DefaultAuthenticationSchema)
                .AddJwtBearer(DefaultAuthenticationSchema, options =>
                {
                    options.Authority = authority;
                    options.RequireHttpsMetadata = false;
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
            
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IDateTimeService, DateTimeService>();
            
            services.AddSingleton<IAuthorizationHandler, EntityOwnerAuthorizationHandler>();

            services.AddOpenApiDocument(configure =>
            {
                configure.Title = "Investing Management API";
                configure.AddSecurity("JWT", Enumerable.Empty<string>(), new OpenApiSecurityScheme
                {
                    Type = OpenApiSecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Description = "Type into the text box: Bearer {your JWT token}."
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
            
            app.UseStaticFiles();

            app.UseSwaggerUi3(settings =>
            {
                settings.Path = "/api";
                settings.DocumentPath = "/api/specification.json";
            });

            app.UseHttpsRedirection();
            
            app.UseCors();

            app.UseRouting();
            

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}