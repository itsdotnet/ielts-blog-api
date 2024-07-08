using IELTSBlog.Domain.Commons;

namespace IELTSBlog.Domain.Entities;

public class Category : Auditable
{
    public string Title { get; set; }
    public long? ParentId { get; set; }
}