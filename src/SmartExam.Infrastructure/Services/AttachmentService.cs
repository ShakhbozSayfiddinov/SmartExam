using Microsoft.EntityFrameworkCore;
using SmartExam.Application.DTOs.Attachments;
using SmartExam.Application.Interfaces;
using SmartExam.Application.Models;
using SmartExam.Domain.Entities;
using SmartExam.Domain.Exceptions;
using SmartExam.Infrastructure.Persistence;

namespace SmartExam.Infrastructure.Services;

public class AttachmentService(AppDbContext context) : IAttachmentService
{
    public async Task<List<AttachmentModel>> GetAllAsync()
    {
        var attachments = await context.Attachments
            .Include(a => a.Zone)
            .Where(a => !a.IsDeleted)
            .ToListAsync();

        return attachments.Select(AttachmentModel.MapFromEntity)?.ToList();
    }

    public async Task<AttachmentModel> GetByIdAsync(Guid id)
    {
        var attachment = await context.Attachments
            .Include(a => a.Zone)
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted);

        return attachment is null ? null : AttachmentModel.MapFromEntity(attachment);
    }

    public async Task<AttachmentModel> CreateAsync(CreateAttachmentDto dto)
    {
        var zoneExists = await context.Zones.AnyAsync(z => z.Id == dto.ZoneId && !z.IsDeleted);
        if (!zoneExists)
            throw new SmartExamException(404, "error_zone_not_found");

        var attachment = new Attachment
        {
            FileName    = dto.FileName,
            ContentType = dto.ContentType,
            FileSize    = dto.FileSize,
            Url         = dto.Url,
            ZoneId      = dto.ZoneId,
        };

        await context.Attachments.AddAsync(attachment);
        await context.SaveChangesAsync();
        await context.Entry(attachment).Reference(a => a.Zone).LoadAsync();

        return AttachmentModel.MapFromEntity(attachment);
    }

    public async Task<AttachmentModel> UpdateAsync(Guid id, UpdateAttachmentDto dto)
    {
        var attachment = await context.Attachments
            .Include(a => a.Zone)
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted)
            ?? throw new SmartExamException(404, "error_attachment_not_found");

        var zoneExists = await context.Zones.AnyAsync(z => z.Id == dto.ZoneId && !z.IsDeleted);
        if (!zoneExists)
            throw new SmartExamException(404, "error_zone_not_found");

        attachment.FileName    = dto.FileName;
        attachment.ContentType = dto.ContentType;
        attachment.FileSize    = dto.FileSize;
        attachment.Url         = dto.Url;
        attachment.ZoneId      = dto.ZoneId;
        attachment.UpdatedAt   = DateTime.UtcNow;

        await context.SaveChangesAsync();
        await context.Entry(attachment).Reference(a => a.Zone).LoadAsync();

        return AttachmentModel.MapFromEntity(attachment);
    }

    public async Task DeleteAsync(Guid id)
    {
        var attachment = await context.Attachments
            .FirstOrDefaultAsync(a => a.Id == id && !a.IsDeleted)
            ?? throw new SmartExamException(404, "error_attachment_not_found");

        attachment.IsDeleted = true;
        attachment.UpdatedAt = DateTime.UtcNow;

        await context.SaveChangesAsync();
    }
}
