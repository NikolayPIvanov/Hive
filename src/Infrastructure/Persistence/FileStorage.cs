using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Azure.Core;
using Azure.Storage.Blobs.Models;
using Hive.Application.Common.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace Hive.Infrastructure.Persistence
{
    using Azure.Storage.Blobs;

    public class FileServiceSettings
    {
        public string BlobConnectionString { get; set; }

        public string BlobContainerName { get; set; }
    }

    
    public class FileService : IFileService
    {
        private readonly IOptions<FileServiceSettings> _settings;
        private readonly BlobContainerClient _blobContainerClient;
        
        public FileService(IOptions<FileServiceSettings> settings)
        {
            _settings = settings;
            _blobContainerClient =
                new BlobContainerClient(settings.Value.BlobConnectionString, settings.Value.BlobContainerName);
        }

        public async Task<string> UploadAsync(IFormFile formFile)
        {
            try
            {
                var extension = Path.GetExtension(formFile.FileName);
                await _blobContainerClient.CreateIfNotExistsAsync();
                var blobName = $"{Randomize()}{extension}";
                var blob = _blobContainerClient.GetBlobClient(blobName);
                await using var stream = formFile.OpenReadStream();
                await blob.UploadAsync(stream);

                return blobName;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public async Task<FileResponse> DownloadAsync(string location)
        {
            try
            {
                await _blobContainerClient.CreateIfNotExistsAsync(default);
                var blobClient = _blobContainerClient.GetBlobClient(location);
                var response = await blobClient.DownloadAsync();

                var extension = location.Split(".").Last();

                return new FileResponse(response.Value.Content, response.Value.ContentType,
                    $"{Guid.NewGuid()}.{extension}");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public string Randomize(string prefix = "sample") =>
            $"{prefix}-{Guid.NewGuid()}";
    }
}