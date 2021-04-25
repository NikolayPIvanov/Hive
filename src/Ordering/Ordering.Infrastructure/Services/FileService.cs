using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
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
        
        public async Task<string> UploadAsync(Stream fileStream, string extension, CancellationToken cancellationToken)
        {
            await _blobContainerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
            extension = extension.StartsWith(".") ? extension : $".{extension}";
            var blobName = $"{Randomize()}{extension}";
            var blob = _blobContainerClient.GetBlobClient(blobName);
            
            await blob.UploadAsync(fileStream, cancellationToken);

            return blobName;
        }

        public async Task<bool> DeleteAsync(string blobName, CancellationToken cancellationToken)
        {
            await _blobContainerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
            var blobClient = _blobContainerClient.GetBlobClient(blobName);
            var response = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);

            return response.Value;
        }

        public async Task<FileResponse> DownloadAsync(string location, CancellationToken cancellationToken)
        {
            await _blobContainerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
            var blobClient = _blobContainerClient.GetBlobClient(location);
            var response = await blobClient.DownloadAsync(cancellationToken: cancellationToken);

            var extension = location.Split(".").Last();

            return new FileResponse(response.Value.Content, response.Value.ContentType,
                $"{Guid.NewGuid()}.{extension}");
        }
        
        private string Randomize(string prefix = "sample") => $"{prefix}-{Guid.NewGuid()}";
    }
}