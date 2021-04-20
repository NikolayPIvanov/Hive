using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using Ordering.Application.Interfaces;

namespace Ordering.Infrastructure.Services
{
    using Azure.Storage.Blobs;

    public record FileServiceSettings(string BlobConnectionString, string BlobContainerName);
    
    public class FileService : IFileService
    {
        private readonly BlobContainerClient _blobContainerClient;
        
        public FileService(IOptions<FileServiceSettings> settings)
        {
            _blobContainerClient =
                new BlobContainerClient(settings.Value.BlobConnectionString, settings.Value.BlobContainerName);
        }

        
        public async Task<string> UploadAsync(IFormFile formFile)
        {
            await _blobContainerClient.CreateIfNotExistsAsync();

            var blobName = $"{Randomize()}{Path.GetExtension(formFile.FileName)}";
            var blob = _blobContainerClient.GetBlobClient(blobName);
            
            await using var stream = formFile.OpenReadStream();
            await blob.UploadAsync(stream);

            return blobName;
        }

        public async Task<bool> DeleteAsync(string blobName)
        {
            await _blobContainerClient.CreateIfNotExistsAsync(default);
            var blobClient = _blobContainerClient.GetBlobClient(blobName);
            var response = await blobClient.DeleteIfExistsAsync();

            return response.Value;
        }

        public async Task<FileResponse> DownloadAsync(string location)
        {
            await _blobContainerClient.CreateIfNotExistsAsync(default);
            var blobClient = _blobContainerClient.GetBlobClient(location);
            var response = await blobClient.DownloadAsync();

            var extension = location.Split(".").Last();

            return new FileResponse(response.Value.Content, response.Value.ContentType,
                $"{Guid.NewGuid()}.{extension}");
        }
        
        private string Randomize(string prefix = "sample") => $"{prefix}-{Guid.NewGuid()}";

    }
}