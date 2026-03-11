using Microsoft.AspNetCore.Http;

namespace SmartExam.Application.Interfaces;

public interface IFileStorageService
{
    Task<string> SaveAsync(IFormFile file);
    void Delete(string url);
}
