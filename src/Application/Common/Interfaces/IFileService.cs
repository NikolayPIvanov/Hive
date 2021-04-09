using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace Hive.Application.Common.Interfaces
{
    public interface IFileService
    {
        Task<string> UploadAsync(IFormFile formFile);
    }
}