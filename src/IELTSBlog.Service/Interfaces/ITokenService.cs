using IELTSBlog.Domain.Entities;

namespace IELTSBlog.Service.Interfaces;

public interface ITokenService
{
    public string GenerateToken(User user);
}