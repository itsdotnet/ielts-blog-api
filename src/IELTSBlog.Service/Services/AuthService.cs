using AutoMapper;
using IELTSBlog.Repository.IRepositories;
using IELTSBlog.Repository.Repository;
using IELTSBlog.Service.DTOs.Users;
using IELTSBlog.Service.Exceptions;
using IELTSBlog.Service.Helpers;
using IELTSBlog.Service.Implementations;
using IELTSBlog.Service.Interfaces;
using Microsoft.Extensions.Caching.Memory;
using System.Numerics;

namespace IELTSBlog.Service.Services;

public class AuthService(
    IUnitOfWork unitOfWork,
    IMapper mapper,
    ITokenService tokenService,
    IMemoryCache memoryCache,
    IUserService userService) : IAuthService
{
    private const int CACHED_MINUTES_FOR_REGISTER = 60;
    private const int CACHED_MINUTES_FOR_VERIFICATION = 5;
    private const string REGISTER_CACHE_KEY = "register_";
    private const string VERIFY_REGISTER_CACHE_KEY = "verify_register_";
    private const int VERIFICATION_MAXIMUM_ATTEMPTS = 3;

    public async Task<string> LoginAsync(string email, string password)
    {
        var user = await unitOfWork.UserRepository.SelectAsync(user =>
            user.Email == email) ?? throw new NotFoundException("User not found.");

        if (!PasswordHasher.Verify(password, user.Password))
            throw new InvalidOperationException("Password is not correct.");

        var token = tokenService.GenerateToken(user);
        return token;
    }

    public async Task<(bool Result, int CashedMinutes)> RegisterAsync(UserCreationDto registerDto)
    {
        if (unitOfWork.UserRepository.SelectAll().Any(user => user.Email == registerDto.Email))
            throw new AlreadyExistException("User already exist with this email!");

        if (memoryCache.TryGetValue(REGISTER_CACHE_KEY + registerDto.Email, out UserCreationDto userCreationDto))
        {
            memoryCache.Remove(REGISTER_CACHE_KEY + registerDto.Email);
        }
        else
        {
            memoryCache.Set(REGISTER_CACHE_KEY + registerDto.Email, registerDto, TimeSpan.FromMinutes(CACHED_MINUTES_FOR_REGISTER));
        }

        return (Result: true, CashedMinutes: CACHED_MINUTES_FOR_REGISTER);
    }

    public Task<(bool Result, int CashedVerificationMinutes)> SendCodeForRegisterAsync(string email)
    {
        throw new NotImplementedException();
    }

    public async Task<(bool Result, string Token)> VerifyRegisterAsync(string email, int code)
    {
        if (await unitOfWork.UserRepository.SelectAsync(x => x.Email == email) is not null)
            throw new AlreadyExistException("This user already registered");

        if (memoryCache.TryGetValue(REGISTER_CACHE_KEY + email, out UserCreationDto dto))
        {
            if (memoryCache.TryGetValue(VERIFY_REGISTER_CACHE_KEY + email, out UserVerficationDto verificationDto))
            {
                if (verificationDto.Attempt >= VERIFICATION_MAXIMUM_ATTEMPTS)
                    throw new CustomException(429, "Too many requests");
                else if (verificationDto.Code == code)
                {
                    var dbResult = await userService.CreateAsync(dto) is null ? false : true;
                    if (dbResult is true)
                    {
                        var user = await unitOfWork.UserRepository.SelectAsync(x => x.Email == email.ToLower());
                        string token = tokenService.GenerateToken(user);
                        memoryCache.Remove(REGISTER_CACHE_KEY + email);
                        return (Result: true, Token: token);
                    }
                    else return (Result: false, Token: "");
                }
                else
                {
                    memoryCache.Remove(VERIFY_REGISTER_CACHE_KEY + email);
                    verificationDto.Attempt++;
                    memoryCache.Set(VERIFY_REGISTER_CACHE_KEY + email, verificationDto,
                        TimeSpan.FromMinutes(CACHED_MINUTES_FOR_VERIFICATION));
                    return (Result: false, Token: "");
                }
            }
            else throw new CustomException(410, "Verfication code time expired");
        }
        else throw new CustomException(410, "Registration time expired");
    }
}
