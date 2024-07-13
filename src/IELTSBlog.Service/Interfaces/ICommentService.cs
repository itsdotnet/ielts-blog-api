using IELTSBlog.Domain.Configrations;
using IELTSBlog.Service.DTOs.Comments;

namespace IELTSBlog.Service.Interfaces;

public interface ICommentService
{
    Task<bool> DeleteAsync(long id);
    Task<CommentResultDto> GetByIdAsync(long id);
    Task<CommentResultDto> UpdateAsync(CommentUpdateDto dto);
    Task<CommentResultDto> CreateAsync(CommentCreationDto dto);
    Task<IEnumerable<CommentResultDto>> GetAllAsync(PaginationParams @params);
}