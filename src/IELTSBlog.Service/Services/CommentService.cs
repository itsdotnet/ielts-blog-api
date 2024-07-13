using AutoMapper;
using IELTSBlog.Domain.Configrations;
using IELTSBlog.Domain.Entities;
using IELTSBlog.Repository.IRepositories;
using IELTSBlog.Service.DTOs.Comments;
using IELTSBlog.Service.Exceptions;
using IELTSBlog.Service.Interfaces;

namespace IELTSBlog.Service.Services;

public class CommentService(
    IUnitOfWork unitOfWork,
    IMapper mapper) : ICommentService
{
    public async Task<CommentResultDto> CreateAsync(CommentCreationDto dto)
    {
        var comment = mapper.Map<Comment>(dto);
        await unitOfWork.CommentRepository.AddAsync(comment);
        await unitOfWork.SaveAsync();

        return mapper.Map<CommentResultDto>(comment);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var comment = await unitOfWork.CommentRepository.SelectAsync(comment =>
            comment.Id == id)
                ?? throw new NotFoundException("Comment not found");

        await unitOfWork.CommentRepository.DeleteAsync(dbComment => dbComment == comment);
        await unitOfWork.SaveAsync();

        return true;
    }

    public async Task<IEnumerable<CommentResultDto>> GetAllAsync(PaginationParams @params)
    {
        var comments = unitOfWork.CommentRepository
            .SelectAll(includes: new string[]
            {
                nameof(Comment.User),
                nameof(Comment.Article),
                nameof(Comment.Parent)
            }).AsEnumerable();

        return mapper.Map<IEnumerable<CommentResultDto>>(comments);
    }

    public async Task<CommentResultDto> GetByIdAsync(long id)
    {
        var comment = await unitOfWork.CommentRepository.SelectAsync(comment =>
            comment.Id == id, new string[] { nameof(Comment.User), nameof(Comment.Article), nameof(Comment.Parent) })
                ?? throw new NotFoundException("Comment not found.");

        return mapper.Map<CommentResultDto>(comment);
    }

    public async Task<CommentResultDto> UpdateAsync(CommentUpdateDto dto)
    {
        var existingComment = await unitOfWork.CommentRepository.SelectAsync(comment =>
            comment.Id == dto.Id) ?? throw new NotFoundException("Comment not found.");

        existingComment.Content = dto.Content;
        await unitOfWork.CommentRepository.UpdateAsync(existingComment);
        await unitOfWork.SaveAsync();

        return mapper.Map<CommentResultDto>(existingComment);
    }
}
