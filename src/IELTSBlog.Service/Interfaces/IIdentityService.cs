using IELTSBlog.Service.DTOs.Users;

namespace IELTSBlog.Service.Interfaces;

public interface IIdentityService
{
    Task<string> CurrentRole();
    Task<UserResultDto> CurrentUser();
}