using AutoMapper;
using IELTSBlog.Repository.IRepositories;
using IELTSBlog.Service.DTOs.Users;
using IELTSBlog.Service.Exceptions;
using IELTSBlog.Service.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace IELTSBlog.Service.Services;

public class IdentityService(
    IMapper mapper,
    IUnitOfWork unitOfWork,
    IHttpContextAccessor httpContextAccessor) : IIdentityService
{
    public Task<string> CurrentRole() =>
        Task.FromResult(httpContextAccessor.HttpContext!.User.Claims
            .FirstOrDefault(claim => claim.Type == ClaimTypes.Role)!.Value);

    public async Task<UserResultDto> CurrentUser()
    {
        var userId = long.Parse(httpContextAccessor.HttpContext!.User.Claims.FirstOrDefault(claim =>
            claim.Type == "Id")!.Value);

        var user = await unitOfWork.UserRepository.SelectAsync(user =>
            user.Id == userId) ?? throw new NotFoundException("User not found.");

        return mapper.Map<UserResultDto>(user);
    }
}
