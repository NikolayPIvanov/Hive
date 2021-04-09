using System;
using System.Collections.Generic;
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
        private readonly BlobContainerClient _blobContainerClient;
        
        public FileService(IOptions<FileServiceSettings> settings)
        {
            _blobContainerClient =
                new BlobContainerClient(settings.Value.BlobConnectionString, settings.Value.BlobContainerName);
        }

        public async Task<string> UploadAsync(IFormFile formFile)
        {
            try
            {
                var blob = _blobContainerClient.GetBlobClient(Randomize());
                await using var stream = formFile.OpenReadStream();
                await blob.UploadAsync(stream);
                
                // Verify we uploaded some content
                BlobProperties properties = await blob.GetPropertiesAsync();
                return properties.CopySource.ToString();
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