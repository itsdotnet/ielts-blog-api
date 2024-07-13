using IELTSBlog.Domain.Commons;

namespace IELTSBlog.Domain.Entities;

public class Comment : Auditable
{
    public string Content { get; set; }                 
    
    public long UserId { get; set; }
    public User User { get; set; }
    
    public long ArticleId { get; set; }
    public Article Article { get; set; }
    
    public long? ParentId { get; set; }
    public Comment Parent { get; set; } 
}