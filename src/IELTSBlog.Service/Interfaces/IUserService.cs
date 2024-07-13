using IELTSBlog.Service.DTOs.Users;

namespace IELTSBlog.Service.Interfaces;

public interface IUserService
{
    Task<bool> DeleteAsync(long id);
    Task<UserResultDto> GetByIdAsync(long id);
    Task<IEnumerable<UserResultDto>> GetAllAsync();
    Task<UserResultDto> UpdateAsync(UserUpdateDto dto);
    Task<UserResultDto> CreateAsync(UserCreationDto dto);
    Task<UserResultDto> UpdatePasswordAsync(long id, string oldPass, string newPass);
}