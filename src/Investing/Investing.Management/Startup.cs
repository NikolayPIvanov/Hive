using FluentValidation.AspNetCore;
using Hive.Common.Core;
using Hive.Common.Core.Filters;
using Hive.Common.Core.Identity;
using Hive.Common.Core.Interfaces;
using Hive.Common.Core.Security.Requirements;
using Hive.Common.Core.Services;
using Hive.Investing.Application;
using Hive.Investing.Infrastructure;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

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
            
            services.AddInvestingInfrastructure(Configuration);
            services.AddInvestingApplication();
            
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
            
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo {Title = "Investing.Management", Version = "v1"});
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "Investing.Management v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });
        }
    }
}