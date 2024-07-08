using IELTSBlog.Domain.Entities;

namespace IELTSBlog.Repository.IRepositories;

public interface IUnitOfWork : IDisposable
{
    IRepository<User> UserRepository { get; }
    IRepository<Article> ArticleRepository { get; }
    IRepository<Category> CategoryRepository { get; }
    IRepository<Comment> CommentRepository { get; }
    Task<bool> SaveAsync();
}