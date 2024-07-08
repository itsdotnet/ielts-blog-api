namespace IELTSBlog.Service.DTOs.Categories;

public class CategoryUpdateDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public long? ParentId { get; set; }
}