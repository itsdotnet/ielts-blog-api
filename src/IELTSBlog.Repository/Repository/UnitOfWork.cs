using IELTSBlog.Domain.Entities;
using IELTSBlog.Repository.Contexts;
using IELTSBlog.Repository.IRepositories;

namespace IELTSBlog.Repository.Repository;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _dbContext;

    public UnitOfWork(AppDbContext dbContext)
    {
        _dbContext = dbContext;

        UserRepository = new Repository<User>(dbContext);
        ArticleRepository = new Repository<Article>(dbContext);
        CategoryRepository = new Repository<Category>(dbContext);
        CommentRepository = new Repository<Comment>(dbContext);
        AttachmentRepository = new Repository<Attachment>(dbContext);
    }

    public IRepository<User> UserRepository { get; }
    public IRepository<Article> ArticleRepository { get; }
    public IRepository<Category> CategoryRepository { get; }
    public IRepository<Comment> CommentRepository { get; }
    public IRepository<Attachment> AttachmentRepository { get; }

    public void Dispose()
    {
        GC.SuppressFinalize(true);
    }

    public async Task<bool> SaveAsync()
    {
        var result = await _dbContext.SaveChangesAsync();
        return result > 0;
    }
}