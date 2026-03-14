using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SmartExam.Application.Interfaces;

namespace SmartExam.Infrastructure.Services;

public class LocalFileStorageService(IWebHostEnvironment env) : IFileStorageService
{
    private const string UploadFolder = "uploads";

    public async Task<string> SaveAsync(IFormFile file)
    {
        string webRoot     = env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot");
        string uploadsPath = Path.Combine(webRoot, UploadFolder);
        Directory.CreateDirectory(uploadsPath);

        string extension = Path.GetExtension(file.FileName);
        string fileName  = $"{Guid.NewGuid()}{extension}";
        string filePath  = Path.Combine(uploadsPath, fileName);

        await using var stream = File.Create(filePath);
        await file.CopyToAsync(stream);

        return $"/{UploadFolder}/{fileName}";
    }

    public void Delete(string url)
    {
        if (string.IsNullOrWhiteSpace(url)) return;

        string webRoot  = env.WebRootPath ?? Path.Combine(env.ContentRootPath, "wwwroot");
        string filePath = Path.Combine(webRoot, url.TrimStart('/'));
        if (File.Exists(filePath))
            File.Delete(filePath);
    }
}
