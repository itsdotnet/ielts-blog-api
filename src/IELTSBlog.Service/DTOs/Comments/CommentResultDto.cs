using IELTSBlog.Service.DTOs.Articles;
using IELTSBlog.Service.DTOs.Users;

namespace IELTSBlog.Service.DTOs.Comments;

public class CommentResultDto
{
    public long Id { get; set; } 
    public string Content { get; set; }                 
    public UserResultDto User { get; set; } 
    public ArticleResultDto Article { get; set; }
    public CommentResultDto Parent { get; set; }
}