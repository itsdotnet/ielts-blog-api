using Microsoft.AspNetCore.Http;

namespace IELTSBlog.Service.DTOs.Articles;

public class ArticleUpdateDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public long CategoryId { get; set; }
    public IFormFile File { get; set; }
}