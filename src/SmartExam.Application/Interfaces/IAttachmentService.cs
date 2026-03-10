using SmartExam.Application.DTOs.Attachments;
using SmartExam.Application.Models;

namespace SmartExam.Application.Interfaces;

public interface IAttachmentService
{
    Task<List<AttachmentModel>> GetAllAsync();
    Task<AttachmentModel> GetByIdAsync(Guid id);
    Task<AttachmentModel> CreateAsync(CreateAttachmentDto dto);
    Task<AttachmentModel> UpdateAsync(Guid id, UpdateAttachmentDto dto);
    Task DeleteAsync(Guid id);
}
