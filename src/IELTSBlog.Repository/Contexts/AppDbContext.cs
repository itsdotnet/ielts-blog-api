using IELTSBlog.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace IELTSBlog.Repository.Contexts;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }
    
    public DbSet<User> Users { get; set; }
    public DbSet<Article> Videos { get; set; }
    public DbSet<Category> Images { get; set; }
    public DbSet<Comment> Courses { get; set; }
}