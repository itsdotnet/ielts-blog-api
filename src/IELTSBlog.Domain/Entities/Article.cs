using IELTSBlog.Domain.Commons;

namespace IELTSBlog.Domain.Entities;

public class Article : Auditable
{
    public string Title { get; set; }
    public string Content { get; set; }
    
    public long CategoryId { get; set; }
    public Category Category { get; set; }
    
    public long UserId { get; set; }
    public User User { get; set; }
}