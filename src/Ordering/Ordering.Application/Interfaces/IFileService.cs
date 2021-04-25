using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Application.Interfaces
{
    public record FileResponse(Stream Source, string ContentType, string FileName);

    public interface IFileService
    {
        Task<string> UploadAsync(Stream fileStream, string extension, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(string blobName, CancellationToken cancellationToken = default);
        Task<FileResponse> DownloadAsync(string location, CancellationToken cancellationToken = default);
    }
}