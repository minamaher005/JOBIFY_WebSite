using Microsoft.AspNetCore.Http;

namespace WebApplication2.Interfaces
{
    public interface IFileUploadService
    {
        Task<string> UploadFileAsync(IFormFile file, string subfolder, string fileNamePrefix);
        bool DeleteFile(string relativePath);
    }
}
