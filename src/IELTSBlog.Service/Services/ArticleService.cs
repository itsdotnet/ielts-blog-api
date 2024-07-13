using AutoMapper;
using IELTSBlog.Domain.Configrations;
using IELTSBlog.Domain.Entities;
using IELTSBlog.Repository.IRepositories;
using IELTSBlog.Service.DTOs.Articles;
using IELTSBlog.Service.DTOs.Attachments;
using IELTSBlog.Service.Exceptions;
using IELTSBlog.Service.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace IELTSBlog.Service.Services;

public class ArticleService : IArticleService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IAttachmentService _attachmentService;

    public ArticleService(IUnitOfWork unitOfWork, IMapper mapper, IAttachmentService attachmentService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _attachmentService = attachmentService;
    }

    public async Task<ArticleResultDto> CreateAsync(ArticleCreationDto dto)
    {
        var article = _mapper.Map<Article>(dto);
        var image = new Attachment();
        if (dto.File is null)
            throw new ArgumentNullException();

        image = await _attachmentService.UploadAsync(new AttachmentCreationDto { File = dto.File }, "images");

        article.AttachmentId = image.Id;
        await _unitOfWork.ArticleRepository.AddAsync(article);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<ArticleResultDto>(article);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var article = await _unitOfWork.ArticleRepository.SelectAsync(article => article.Id == id)
            ?? throw new NotFoundException("Article not found");

        await _unitOfWork.ArticleRepository.DeleteAsync(x => x == article);
        return await _unitOfWork.SaveAsync();
    }

    public async Task<IEnumerable<ArticleResultDto>> GetAllAsync(PaginationParams @params)
    {
        var articles = _unitOfWork.ArticleRepository.SelectAll(includes: new string[]
        {
            nameof(Article.User),
            nameof(Article.Category),
            nameof(Article.Attachment)
        });

        return _mapper.Map<IEnumerable<ArticleResultDto>>(articles);
    }

    public async Task<ArticleResultDto> GetByIdAsync(long id)
    {
        var article = await _unitOfWork.ArticleRepository.SelectAsync(article =>
            article.Id == id, includes: new string[]
            {
                nameof(Article.User),
                nameof(Article.Attachment),
                nameof(Article.Category)
            }) ?? throw new NotFoundException("Article not found.");

        return _mapper.Map<ArticleResultDto>(article);

    }

    public async Task<ArticleResultDto> UpdateAsync(ArticleUpdateDto dto)
    {
        var existingArticle = await _unitOfWork.ArticleRepository.SelectAsync(article =>
            article.Id == dto.Id) ?? throw new NotFoundException("Article not found.");

        var image = new Attachment();

        if (dto.File is not null)
        {
            image = await _attachmentService.UploadAsync(new AttachmentCreationDto
            {
                File = dto.File
            }, "images");

            existingArticle.AttachmentId = image.Id;
        }

        _mapper.Map(dto, existingArticle);
        await _unitOfWork.ArticleRepository.UpdateAsync(existingArticle);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<ArticleResultDto>(existingArticle);
    }
}
