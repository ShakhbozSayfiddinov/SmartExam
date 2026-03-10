using Microsoft.EntityFrameworkCore;
using SmartExam.Application.DTOs.Zones;
using SmartExam.Application.Interfaces;
using SmartExam.Application.Models;
using SmartExam.Domain.Entities;
using SmartExam.Domain.Exceptions;
using SmartExam.Infrastructure.Persistence;

namespace SmartExam.Infrastructure.Services;

public class ZoneService(AppDbContext context) : IZoneService
{
    public async Task<List<ZoneModel>> GetAllAsync()
    {
        var zones = await context.Zones
            .Where(z => !z.IsDeleted)
            .ToListAsync();

        return zones.Select(ZoneModel.MapFromEntity)?.ToList();
    }

    public async Task<ZoneModel> GetByIdAsync(Guid id)
    {
        var zone = await context.Zones
            .FirstOrDefaultAsync(z => z.Id == id && !z.IsDeleted);

        return zone is null ? null : ZoneModel.MapFromEntity(zone);
    }

    public async Task<ZoneModel> CreateAsync(CreateZoneDto dto)
    {
        var zone = new Zone
        {
            Name   = dto.Name,
            Width  = dto.Width,
            Height = dto.Height,
        };

        await context.Zones.AddAsync(zone);
        await context.SaveChangesAsync();

        return ZoneModel.MapFromEntity(zone);
    }

    public async Task<ZoneModel> UpdateAsync(Guid id, UpdateZoneDto dto)
    {
        var zone = await context.Zones
            .FirstOrDefaultAsync(z => z.Id == id && !z.IsDeleted)
            ?? throw new SmartExamException(404, "error_zone_not_found");

        zone.Name      = dto.Name;
        zone.Width     = dto.Width;
        zone.Height    = dto.Height;
        zone.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();

        return ZoneModel.MapFromEntity(zone);
    }

    public async Task DeleteAsync(Guid id)
    {
        var zone = await context.Zones
            .FirstOrDefaultAsync(z => z.Id == id && !z.IsDeleted)
            ?? throw new SmartExamException(404, "error_zone_not_found");

        zone.IsDeleted = true;
        zone.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }
}
