using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Hive.Application.Common.Interfaces
{
    public record FileResponse(Stream Source, string ContentType, string FileName);

    public interface IFileService
    {
        Task<string> UploadAsync(IFormFile formFile);
        Task<FileResponse> DownloadAsync(string location);
    }
}