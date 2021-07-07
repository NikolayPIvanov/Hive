using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace BuildingBlocks.Core.Interfaces
{
    public record FileResponse(Stream Source, string ContentType, string FileName);

    public interface IFileService
    {
        Task<string> UploadAsync(string blobContainerName, Stream fileStream, string extension, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string blobContainerName, string blobName, CancellationToken cancellationToken = default);
        Task<FileResponse> DownloadAsync(string blobContainerName, string location, CancellationToken cancellationToken = default);
    }
}