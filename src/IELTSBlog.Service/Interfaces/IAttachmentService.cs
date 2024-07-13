using IELTSBlog.Domain.Entities;
using IELTSBlog.Service.DTOs.Attachments;

namespace IELTSBlog.Service.Interfaces;

public interface IAttachmentService
{
    Task<bool> DeleteAsync(long id);
    Task<Attachment> UploadAsync(AttachmentCreationDto dto, string folder);
}
