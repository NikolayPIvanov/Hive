// Copyright (c) Duende Software. All rights reserved.
// See LICENSE in the project root for license information.


using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;
using System;
using System.Linq;
using System.Threading.Tasks;
using Duende.IdentityServer.EntityFramework.DbContexts;
using Duende.IdentityServer.EntityFramework.Mappers;
using Hive.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Serilog.Sinks.Elasticsearch;

namespace Hive.Identity
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();

            using var scope = host.Services.CreateScope();
            var services = scope.ServiceProvider;
            
            var configuration = services.GetService<IConfiguration>();

            var elasticUri = configuration.GetValue<string>("ElasticUrl");
            
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.Elasticsearch(new ElasticsearchSinkOptions(new Uri(elasticUri) ){
                    AutoRegisterTemplate = true,
                    AutoRegisterTemplateVersion = AutoRegisterTemplateVersion.ESv6
                })
                .WriteTo.Console(
                    outputTemplate:
                    "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}",
                    theme: AnsiConsoleTheme.Code)
                .CreateLogger();

            try
            {
                InitializeDatabase(services);
                
                await host.RunAsync();
            }
            catch (Exception ex)
            {
                Log.Fatal(ex, "Host terminated unexpectedly.");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .UseSerilog()
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                    webBuilder.UseUrls("http://localhost:7001");
                });
        
        private static void InitializeDatabase(IServiceProvider app)
        {
            using var serviceScope = app.GetService<IServiceScopeFactory>()?.CreateScope();
            if (serviceScope == null) return;
            InitializePersistedGrantDbContext(serviceScope);
            InitializeConfigurationDbContext(serviceScope);
            InitializeApplicationDbContext(serviceScope);
        }

        private static  void InitializePersistedGrantDbContext(IServiceScope serviceScope)
        {
            serviceScope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
        }
        
        private static void InitializeConfigurationDbContext(IServiceScope serviceScope)
        {
            var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();
            context.Database.Migrate();

            var allClients = context.Clients.AsQueryable();
            context.Clients.RemoveRange(allClients);
            context.Clients.AddRange(Config.Clients.Select(x => x.ToEntity()));
            
            var allResources = context.IdentityResources.AsQueryable();
            context.IdentityResources.RemoveRange(allResources);
            context.IdentityResources.AddRange(Config.IdentityResources.Select(x => x.ToEntity()));
            
            var allScopes = context.ApiScopes.AsQueryable();
            context.ApiScopes.RemoveRange(allScopes);
            context.ApiScopes.AddRange(Config.ApiScopes.Select(x => x.ToEntity()));

            var allApiResources = context.ApiResources.AsQueryable();
            context.ApiResources.RemoveRange(allApiResources);
            context.ApiResources.AddRange(Config.ApiResources.Select(x => x.ToEntity()));
            
            context.SaveChanges();
        }
        
        private static void InitializeApplicationDbContext(IServiceScope serviceScope)
        {
            var appContext = serviceScope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            
            if (appContext.Database.IsSqlServer())
            {
                 appContext.Database.Migrate();
            }
        }
    }
}