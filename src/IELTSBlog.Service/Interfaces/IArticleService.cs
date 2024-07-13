using IELTSBlog.Domain.Configrations;
using IELTSBlog.Service.DTOs.Articles;

namespace IELTSBlog.Service.Interfaces;

public interface IArticleService
{
    Task<bool> DeleteAsync(long id);
    Task<ArticleResultDto> GetByIdAsync(long id);
    Task<ArticleResultDto> UpdateAsync(ArticleUpdateDto dto);
    Task<ArticleResultDto> CreateAsync(ArticleCreationDto dto);
    Task<IEnumerable<ArticleResultDto>> GetAllAsync(PaginationParams @params);
}