namespace IELTSBlog.Service.DTOs.Categories;

public class CategoryCreationDto
{
    public string Title { get; set; }
    public long? ParentId { get; set; }
}