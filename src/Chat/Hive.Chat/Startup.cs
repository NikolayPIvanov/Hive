using System.Reflection;
using System.Threading.Tasks;
using Hive.Chat.Data;
using Hive.Chat.Hubs;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;

namespace Hive.Chat
{
    public record RabbitMqSettings
    {
        public string Hostname { get; init; }
        public int Port { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string VirtualHost { get; set; }
    }
    
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

            services.AddControllers();

            services.AddSignalR();

            services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .WithOrigins("http://localhost:4200", "http://localhost:4200")
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .SetIsOriginAllowed(_ => true)
                        .AllowAnyMethod();
                });
            });
            
        services.AddAuthentication(options =>
        {
            // Identity made Cookie authentication the default.
            // However, we want JWT Bearer Auth to be the default.
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            // Configure the Authority to the expected value for your authentication provider
            // This ensures the token is appropriately validated
            options.Authority = authority;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateAudience = false
            };

            // We have to hook the OnMessageReceived event in order to
            // allow the JWT authentication handler to read the access
            // token from the query string when a WebSocket or 
            // Server-Sent Events request comes in.

            // Sending the access token in the query string is required due to
            // a limitation in Browser APIs. We restrict it to only calls to the
            // SignalR hub in this code.
            // See https://docs.microsoft.com/aspnet/core/signalr/security#access-token-logging
            // for more information about security considerations when using
            // the query string to transmit the access token.
            options.Events = new JwtBearerEvents
            {
                OnMessageReceived = context =>
                {
                    var accessToken = context.Request.Query["access_token"];

                    // If the request is for our hub...
                    var path = context.HttpContext.Request.Path;
                    if (!string.IsNullOrEmpty(accessToken) &&
                        (path.StartsWithSegments("/hubs/chat")))
                    {
                        // Read the token out of the query string
                        context.Token = accessToken;
                    }
                    return Task.CompletedTask;
                }
            };
            
        });

        services.AddTransient<IChatContext, ChatContext>();
        services.Configure<ChatDatabaseSettings>(Configuration.GetSection(nameof(ChatDatabaseSettings)));

        services.AddCap(options =>
        {
            var rabbitMqSettings = Configuration.GetSection(nameof(RabbitMqSettings)).Get<RabbitMqSettings>();
            var mongoDbSettings = Configuration.GetSection(nameof(ChatDatabaseSettings)).Get<ChatDatabaseSettings>();

            options.UseMongoDB(opt =>
            {
                opt.DatabaseConnection = mongoDbSettings.ConnectionString;
            });

            options.UseRabbitMQ(ro =>
            {
                ro.Password = rabbitMqSettings.Password;
                ro.UserName = rabbitMqSettings.UserName;
                ro.HostName = rabbitMqSettings.Hostname;
                ro.Port = rabbitMqSettings.Port;
                ro.VirtualHost = rabbitMqSettings.VirtualHost;
            });
        });
            
        services.AddSwaggerGen();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });


            app.UseHttpsRedirection();

            app.UseCors();

            app.UseRouting();
            
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHub<ChatHub>("/chat");
            });
        }
    }
}