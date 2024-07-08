namespace IELTSBlog.Service.DTOs.Articles;

public class ArticleCreationDto
{
    public string Title { get; set; }
    public string Content { get; set; }
    public long CategoryId { get; set; }
    public long UserId { get; set; }
}