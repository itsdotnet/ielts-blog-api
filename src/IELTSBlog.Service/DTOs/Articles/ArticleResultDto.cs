using IELTSBlog.Service.DTOs.Attachments;
using IELTSBlog.Service.DTOs.Categories;
using IELTSBlog.Service.DTOs.Users;

namespace IELTSBlog.Service.DTOs.Articles;

public class ArticleResultDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public CategoryResultDto Category { get; set; }
    public UserResultDto User { get; set; }
    public AttachmentResultDto Attachment { get; set; }
}