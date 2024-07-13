using System.ComponentModel.DataAnnotations;
using AutoMapper;
using IELTSBlog.Domain.Entities;
using IELTSBlog.Repository.IRepositories;
using IELTSBlog.Service.DTOs.Users;
using IELTSBlog.Service.Exceptions;
using IELTSBlog.Service.Helpers;
using IELTSBlog.Service.Interfaces;

namespace IELTSBlog.Service.Implementations;

public class UserService : IUserService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public UserService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var user = await _unitOfWork.UserRepository.SelectAsync(q => q.Id == id);

        if (user is null)
            throw new NotFoundException("User not found");
        
        await _unitOfWork.UserRepository.DeleteAsync(x => x == user);
        return await _unitOfWork.SaveAsync();
    }

    public async Task<UserResultDto> GetByIdAsync(long id)
    {
        var user = await _unitOfWork.UserRepository.SelectAsync(q => q.Id == id);

        if (user is null)
            throw new NotFoundException("User not found");
        
        return _mapper.Map<UserResultDto>(user);
    }

    public async Task<IEnumerable<UserResultDto>> GetAllAsync()
    {
        var users = (IEnumerable<User>)_unitOfWork.UserRepository.SelectAll();

        return _mapper.Map<IEnumerable<UserResultDto>>(users);
    }

    public async Task<UserResultDto> UpdateAsync(UserUpdateDto dto)
    {   
        var exist = await _unitOfWork.UserRepository.SelectAsync(d => d.Id == dto.Id);
    
        if (exist is null)
            throw new NotFoundException("User not found");
        
        _mapper.Map(dto, exist);

        await _unitOfWork.UserRepository.UpdateAsync(exist);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<UserResultDto>(exist);
    }

    public async Task<UserResultDto> CreateAsync(UserCreationDto dto)
    {
        var exist = await _unitOfWork.UserRepository.SelectAsync(q => q.Email == dto.Email);

        if (exist is not null)
            throw new AlreadyExistException("User already exist with this Email");
        
        dto.Password = PasswordHasher.Hash(dto.Password);        
        var newUser = _mapper.Map<User>(dto);
        await _unitOfWork.UserRepository.AddAsync(newUser);
        await _unitOfWork.SaveAsync();

        return _mapper.Map<UserResultDto>(newUser);
    }   

    public async Task<UserResultDto> UpdatePasswordAsync(long id, string oldPass, string newPass)
    {
        if(oldPass == newPass)
            throw new CustomException(400, "Password cant be equal to old password");
            
        var exist = await _unitOfWork.UserRepository.SelectAsync(q => q.Id == id);

        if (exist is null)
            throw new NotFoundException("User not found");

        var isCorrect = PasswordHasher.Verify(oldPass, exist.Password);
        if(!isCorrect)    
            throw new CustomException(401, $"Password {oldPass} is invalid");
        
        exist.Password = newPass.Hash();
        await _unitOfWork.SaveAsync();

        return _mapper.Map<UserResultDto>(exist);
    }
}