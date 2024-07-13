using IELTSBlog.Domain.Configrations;
using IELTSBlog.Service.DTOs.Categories;

namespace IELTSBlog.Service.Interfaces;

public interface ICategoryService
{
    Task<bool> DeleteAsync(long id);
    Task<CategoryResultDto> GetByIdAsync(long id);
    Task<CategoryResultDto> UpdateAsync(CategoryUpdateDto dto);
    Task<CategoryResultDto> CreateAsync(CategoryCreationDto dto);
    Task<IEnumerable<CategoryResultDto>> GetAllAsync(PaginationParams @params);
}