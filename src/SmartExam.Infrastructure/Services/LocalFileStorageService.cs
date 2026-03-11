using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SmartExam.Application.Interfaces;

namespace SmartExam.Infrastructure.Services;

public class LocalFileStorageService(IWebHostEnvironment env) : IFileStorageService
{
    private const string UploadFolder = "uploads";

    public async Task<string> SaveAsync(IFormFile file)
    {
        string uploadsPath = Path.Combine(env.WebRootPath, UploadFolder);
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

        string filePath = Path.Combine(env.WebRootPath, url.TrimStart('/'));
        if (File.Exists(filePath))
            File.Delete(filePath);
    }
}
