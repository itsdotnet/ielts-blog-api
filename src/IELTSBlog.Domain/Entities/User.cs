using IELTSBlog.Domain.Enums;

namespace IELTSBlog.Domain.Entities;

public class User
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public UserRole Role { get; set; }
}