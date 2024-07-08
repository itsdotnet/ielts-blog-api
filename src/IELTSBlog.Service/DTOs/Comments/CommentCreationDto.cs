namespace IELTSBlog.Service.DTOs.Comments;

public class CommentCreationDto
{
    public string Content { get; set; }                 
    public long ArticleId { get; set; }
    public long? ParentId { get; set; }
}