using IELTSBlog.Domain.Constants;

namespace IELTSBlog.Domain.Commons;

public class Auditable
{    
    public long Id { get; set; }
    public DateTime CreatedAt { get; set; } = TimeConstants.Now();
    public DateTime UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }
}