using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using WebApplication2.Interfaces;

namespace WebApplication2.Services.FileUploadService
{
    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly string _uploadsFolder;

        public FileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
            _uploadsFolder = Path.Combine(_environment.WebRootPath, "uploads");
        }

        public async Task<string> UploadFileAsync(IFormFile file, string subfolder, string fileNamePrefix)
        {
            // Create directory if it doesn't exist
            var uploadPath = Path.Combine(_uploadsFolder, subfolder);
            if (!Directory.Exists(uploadPath))
            {
                Directory.CreateDirectory(uploadPath);
            }

            // Generate unique filename
            var fileName = $"{fileNamePrefix}_{DateTime.Now.Ticks}_{Path.GetFileName(file.FileName)}";
            var filePath = Path.Combine(uploadPath, fileName);

            // Save file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // Return relative path for database storage
            return $"/uploads/{subfolder}/{fileName}";
        }

        public bool DeleteFile(string relativePath)
        {
            if (string.IsNullOrEmpty(relativePath))
                return false;

            try
            {
                var fullPath = Path.Combine(_environment.WebRootPath, relativePath.TrimStart('/'));
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }
    }
}
