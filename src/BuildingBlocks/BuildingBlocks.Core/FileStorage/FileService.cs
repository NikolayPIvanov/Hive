using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Azure.Storage.Blobs;
using BuildingBlocks.Core.Interfaces;
using Microsoft.Extensions.Options;

namespace BuildingBlocks.Core.FileStorage
{
    public class FileService : IFileService
    {
        private readonly FileServiceSettings _settings;
        
        public FileService(IOptions<FileServiceSettings> settings)
        {
            _settings = settings.Value;
        }
        
        public async Task<string> UploadAsync(string blobContainerName, Stream fileStream, 
            string extension, CancellationToken cancellationToken)
        {
            var blobContainerClient =
                new BlobContainerClient(_settings.BlobConnectionString, blobContainerName);
            await blobContainerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
            extension = extension.StartsWith(".") ? extension : $".{extension}";
            var blobName = $"{Randomize()}{extension}";
            var blob = blobContainerClient.GetBlobClient(blobName);
            
            await blob.UploadAsync(fileStream, cancellationToken);

            return blobName;
        }

        public async Task<bool> DeleteAsync(string blobContainerName, string blobName, CancellationToken cancellationToken)
        {
            var blobContainerClient =
                new BlobContainerClient(_settings.BlobConnectionString, blobContainerName);
            await blobContainerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
            var blobClient = blobContainerClient.GetBlobClient(blobName);
            var response = await blobClient.DeleteIfExistsAsync(cancellationToken: cancellationToken);

            return response.Value;
        }

        public async Task<FileResponse> DownloadAsync(string blobContainerName, string location, CancellationToken cancellationToken)
        {
            var blobContainerClient =
                new BlobContainerClient(_settings.BlobConnectionString, blobContainerName);
            await blobContainerClient.CreateIfNotExistsAsync(cancellationToken: cancellationToken);
            var blobClient = blobContainerClient.GetBlobClient(location);
            var response = await blobClient.DownloadAsync(cancellationToken);

            var extension = location.Split(".").Last();

            return new FileResponse(response.Value.Content, response.Value.ContentType,
                $"{Guid.NewGuid()}.{extension}");
        }
        
        private string Randomize(string prefix = "sample") => $"{prefix}-{Guid.NewGuid()}";
    }
}