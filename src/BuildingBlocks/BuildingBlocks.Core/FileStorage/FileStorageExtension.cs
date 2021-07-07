using BuildingBlocks.Core.Email;
using BuildingBlocks.Core.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BuildingBlocks.Core.FileStorage
{
    public static class FileStorageExtension
    {
        public static IServiceCollection AddFileStorage(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<FileServiceSettings>(configuration.GetSection(nameof(FileServiceSettings)));
            services.AddScoped<IFileService, FileService>();
            
            return services;
        }
    }
}