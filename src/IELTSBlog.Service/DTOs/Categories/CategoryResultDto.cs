namespace IELTSBlog.Service.DTOs.Categories;

public class CategoryResultDto
{
    public long Id { get; set; }
    public string Title { get; set; }
    public CategoryResultDto Parent { get; set; }
}