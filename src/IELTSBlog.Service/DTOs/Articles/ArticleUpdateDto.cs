namespace IELTSBlog.Service.DTOs.Articles;

public class ArticleUpdateDto
{
    public string Title { get; set; }
    public string Content { get; set; }
    public long CategoryId { get; set; }
}