using AutoMapper;
using IELTSBlog.Repository.IRepositories;
using IELTSBlog.Service.DTOs.Users;
using IELTSBlog.Service.Exceptions;
using IELTSBlog.Service.Interfaces;
using Microsoft.AspNetCore.Http;

namespace IELTSBlog.Service.Implementations;

public class IdentityService : IIdentityService
{
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public IdentityService(IMapper mapper, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<string> CurrentRole()
    {
        return _httpContextAccessor.HttpContext?.User
            .FindFirst("http://schemas.microsoft.com/ws/2008/06/identity/claims/role")?.Value;
    }

    public async Task<UserResultDto> CurrentUser()
    {
        long userId = long.Parse(_httpContextAccessor.HttpContext.User.FindFirst("Id")?.Value);
        var user = await _unitOfWork.UserRepository
            .SelectAsync(x => x.Id == userId);
        if (user is null)
            throw new NotFoundException("User not found");
        
        return _mapper.Map<UserResultDto>(user);
    }
}