using System.ComponentModel.DataAnnotations;
using IELTSBlog.Domain.Constants;
using IELTSBlog.Domain.Entities;
using IELTSBlog.Repository.IRepositories;
using IELTSBlog.Service.DTOs.Users;
using IELTSBlog.Service.Exceptions;
using IELTSBlog.Service.Helpers;
using IELTSBlog.Service.Interfaces;
using Microsoft.Extensions.Caching.Memory;

namespace IELTSBlog.Service.Implementations;


public class AuthService : IAuthService
{
    private const int CACHED_MINUTES_FOR_REGISTER = 60;
    private const int CACHED_MINUTES_FOR_VERIFICATION = 5;
    private const string REGISTER_CACHE_KEY = "register_";
    private const string VERIFY_REGISTER_CACHE_KEY = "verify_register_";
    private const int VERIFICATION_MAXIMUM_ATTEMPTS = 3;
    private readonly IMemoryCache _memoryCache;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ITokenService _tokenService;
    private readonly IUserService _userService;

    public AuthService(IUnitOfWork unitOfWork, ITokenService tokenService, IUserService userService, IMemoryCache memoryCache)
    {
        _unitOfWork = unitOfWork;
        _tokenService = tokenService;
        _userService = userService;
        _memoryCache = memoryCache;
    }

    public async Task<string> LoginAsync(string phone, string password)
    {
        var user = await _unitOfWork.UserRepository.SelectAsync(x => x.Email == phone);
        if (user is null) throw new NotFoundException("User not found");

        var hasherResult = PasswordHasher.Verify(password, user.Password);
        if (hasherResult == false) throw new CustomException(401, "Password has not been verified");

        string token = _tokenService.GenerateToken(user);

        return token;
    }

    public async Task<(bool Result, int CashedMinutes)> RegisterAsync(UserCreationDto registerDto)
    {
        try
        {

            var user = await _unitOfWork.UserRepository.SelectAsync(x => x.Email.Equals(registerDto.Email));
            if (user is not null) throw new AlreadyExistException("User is already exist with this number");

            await _userService.CreateAsync(registerDto);

            return (Result: true, CachedMinutes: CACHED_MINUTES_FOR_REGISTER);
        }
        catch
        {
            return (Result: false, CachedMinutes: 0);
        }
    }

    public async Task<(bool Result, int CashedVerificationMinutes)> SendCodeForRegisterAsync(string phone)
    {
        if (_memoryCache.TryGetValue(REGISTER_CACHE_KEY + phone, out UserCreationDto registerDto))
        {
            UserVerficationDto verificationDto = new UserVerficationDto();
            verificationDto.Attempt = 0;
            verificationDto.CreatedAt = TimeConstants.Now();


            verificationDto.Code = CodeGenerator.GenerateRandomNumber();
            verificationDto.Code = 0000; //delete this line


            if (_memoryCache.TryGetValue(VERIFY_REGISTER_CACHE_KEY + phone, out UserVerficationDto oldDto))
            {
                _memoryCache.Remove(VERIFY_REGISTER_CACHE_KEY + phone);
            }
            _memoryCache.Set(VERIFY_REGISTER_CACHE_KEY + phone, verificationDto,
                TimeSpan.FromMinutes(CACHED_MINUTES_FOR_VERIFICATION));

            //here is the logic for sending sms to phone number
            /*
            EmailMessage emailSms = new EmailMessage();
            emailSms.Title = "Azhar Inc";
            emailSms.Content = "Verification code : " + verificationDto.Code;
            emailSms.Recipent = mail;
        
            var mailResult = await _mailSender.SendAsync(emailSms);
            if (mailResult is true) return (Result: true, CachedVerificationMinutes: CACHED_MINUTES_FOR_VERIFICATION);
            else return (Result: false, CachedVerificationMinutes: 0);
             */
            return (Result: false, CachedVerificationMinutes: 0);
        }
        else throw new CustomException(410, "Registration time expired");
    }

    public async Task<(bool Result, string Token)> VerifyRegisterAsync(string email, int code)
    {
        if (await _unitOfWork.UserRepository.SelectAsync(x => x.Email == email) is not null)
            throw new AlreadyExistException("This user already registered");
        if (_memoryCache.TryGetValue(REGISTER_CACHE_KEY + email, out UserCreationDto dto))
        {
            if (_memoryCache.TryGetValue(VERIFY_REGISTER_CACHE_KEY + email, out UserVerficationDto verificationDto))
            {
                if (verificationDto.Attempt >= VERIFICATION_MAXIMUM_ATTEMPTS)
                    throw new CustomException(429, "Too many requests");
                else if (verificationDto.Code == code)
                {
                    var dbResult = await _userService.CreateAsync(dto) is null ? false : true;
                    if (dbResult is true)
                    {
                        var user = await _unitOfWork.UserRepository.SelectAsync(x => x.Email == email.ToLower());
                        string token = _tokenService.GenerateToken(user);
                        _memoryCache.Remove(REGISTER_CACHE_KEY + email);
                        return (Result: true, Token: token);
                    }
                    else return (Result: false, Token: "");
                }
                else
                {
                    _memoryCache.Remove(VERIFY_REGISTER_CACHE_KEY + email);
                    verificationDto.Attempt++;
                    _memoryCache.Set(VERIFY_REGISTER_CACHE_KEY + email, verificationDto,
                        TimeSpan.FromMinutes(CACHED_MINUTES_FOR_VERIFICATION));
                    return (Result: false, Token: "");
                }
            }
            else throw new CustomException(410, "Verfication code time expired");
        }
        else throw new CustomException(410, "Registration time expired");
    }
}