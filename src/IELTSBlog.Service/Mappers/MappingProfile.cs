using AutoMapper;
using IELTSBlog.Domain.Entities;
using IELTSBlog.Service.DTOs.Articles;
using IELTSBlog.Service.DTOs.Attachments;
using IELTSBlog.Service.DTOs.Categories;
using IELTSBlog.Service.DTOs.Comments;
using IELTSBlog.Service.DTOs.Users;

namespace IELTSBlog.Service.Mappers;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<User, UserResultDto>().ReverseMap();
        CreateMap<UserUpdateDto, User>().ReverseMap();
        CreateMap<UserCreationDto, User>().ReverseMap();
        
        CreateMap<Article,ArticleCreationDto>().ReverseMap();
        CreateMap<Article, ArticleUpdateDto>().ReverseMap();
        CreateMap<ArticleResultDto, Article>().ReverseMap();
        
        CreateMap<Category,CategoryCreationDto>().ReverseMap();
        CreateMap<Category, CategoryUpdateDto>().ReverseMap();
        CreateMap<CategoryResultDto, Category>().ReverseMap();
        
        CreateMap<Comment,CommentCreationDto>().ReverseMap();
        CreateMap<Comment, CommentUpdateDto>().ReverseMap();
        CreateMap<CommentResultDto, Comment>().ReverseMap();

        CreateMap<Attachment, AttachmentResultDto>().ReverseMap();
    }
}