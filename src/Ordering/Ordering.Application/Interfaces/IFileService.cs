using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Ordering.Application.Interfaces
{
    public record FileResponse(Stream Source, string ContentType, string FileName);

    public interface IFileService
    {
        Task<string> UploadAsync(IFormFile formFile);
        Task<bool> DeleteAsync(string blobName);
        Task<FileResponse> DownloadAsync(string location);
    }
}