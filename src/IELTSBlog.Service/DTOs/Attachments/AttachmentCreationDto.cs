using Microsoft.AspNetCore.Http;

namespace IELTSBlog.Service.DTOs.Attachments;

public class AttachmentCreationDto
{
    public IFormFile File { get; set; }
}   
