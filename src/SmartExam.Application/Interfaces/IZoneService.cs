using SmartExam.Application.DTOs.Zones;
using SmartExam.Application.Models;

namespace SmartExam.Application.Interfaces;

public interface IZoneService
{
    Task<List<ZoneModel>> GetAllAsync();
    Task<ZoneModel> GetByIdAsync(Guid id);
    Task<ZoneModel> CreateAsync(CreateZoneDto dto);
    Task<ZoneModel> UpdateAsync(Guid id, UpdateZoneDto dto);
    Task DeleteAsync(Guid id);
}
