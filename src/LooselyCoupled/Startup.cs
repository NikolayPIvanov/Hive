using Billing.Application;
using Billing.Infrastructure;
using FluentValidation.AspNetCore;
using Hive.Common.Core.Behaviours;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Services;
using Hive.Gig.Application;
using Hive.Gig.Infrastructure;
using Hive.Gig.Infrastructure.Services;
using Hive.Investing.Application;
using Hive.Investing.Infrastructure;
using Hive.LooselyCoupled.Authorization.Requirements;
using Hive.LooselyCoupled.Filters;
using Hive.LooselyCoupled.Services;
using Hive.UserProfile.Application;
using Hive.UserProfile.Infrastructure;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Ordering.Application;
using Ordering.Infrastructure;

namespace Hive.LooselyCoupled
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGigsInfrastructure(Configuration);
            services.AddGigsManagement(Configuration);
            
            services.AddOrderingApp(Configuration);
            services.AddOrdering(Configuration);
            
            services.AddBillingInfrastructure(Configuration);
            services.AddBillingApp(Configuration);

            services.AddUserProfileInfrastructure(Configuration);
            services.AddUserProfileApplication(Configuration);

            services.AddInvestingInfrastructure(Configuration);
            services.AddInvestingApplication(Configuration);

            services.AddHttpContextAccessor();

            services.AddControllers(options =>
            {
                options.AllowEmptyInputInBodyModelBinding = true;
                options.Filters.Add<ApiExceptionFilterAttribute>();
            }).AddFluentValidation();

            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IDateTimeService, DateTimeService>();
            
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(PerformanceBehaviour<,>));

            services.AddAuthentication("Bearer")
                .AddJwtBearer("Bearer", options =>
                {
                    options.Authority = "https://localhost:7001";

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
            
            services.AddSingleton<IAuthorizationHandler, EntityOwnerAuthorizationHandler>();

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "LooselyCoupled", Version = "v1"});
                
                c.AddSecurityDefinition("JWT", new OpenApiSecurityScheme()
                {
                    Type = SecuritySchemeType.ApiKey,
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Description = "Type into the textbox: Bearer {your JWT token}."
                });
                
                c.AddSecurityRequirement(new OpenApiSecurityRequirement {
                { 
                    new OpenApiSecurityScheme 
                    { 
                        Reference = new OpenApiReference 
                        { 
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer" 
                        } 
                    },
                    System.Array.Empty<string>()
                } 
                });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LooselyCoupled v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}