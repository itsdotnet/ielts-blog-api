using AutoMapper;
using IELTSBlog.Domain.Entities;
using IELTSBlog.Repository.IRepositories;
using IELTSBlog.Repository.Repository;
using IELTSBlog.Service.DTOs.Users;
using IELTSBlog.Service.Exceptions;
using IELTSBlog.Service.Helpers;
using IELTSBlog.Service.Interfaces;

namespace IELTSBlog.Service.Services;

public class UserService(IUnitOfWork unitOfWork, IMapper mapper) : IUserService
{
    public async Task<UserResultDto> CreateAsync(UserCreationDto dto)
    {
        if (unitOfWork.UserRepository.SelectAll().Any(user => user.Email == dto.Email))
            throw new AlreadyExistException("User already exist with this email");

        dto.Password = PasswordHasher.Hash(dto.Password);
        var newUser = mapper.Map<User>(dto);
        await unitOfWork.UserRepository.AddAsync(newUser);
        await unitOfWork.SaveAsync();

        return mapper.Map<UserResultDto>(newUser);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        var user = await unitOfWork.UserRepository.SelectAsync(user =>
            user.Id == id) ?? throw new NotFoundException("User not found.");

        await unitOfWork.UserRepository.DeleteAsync(x => x == user);
        await unitOfWork.SaveAsync();

        return true;
    }

    public async Task<IEnumerable<UserResultDto>> GetAllAsync()
    {
        var users = unitOfWork.UserRepository.SelectAll().AsEnumerable();
        return mapper.Map<IEnumerable<UserResultDto>>(users);
    }

    public async Task<UserResultDto> GetByIdAsync(long id)
    {
        var user = await unitOfWork.UserRepository.SelectAsync(user =>
            user.Id == id) ?? throw new NotFoundException("User not found.");

        return mapper.Map<UserResultDto>(user);
    }

    public async Task<UserResultDto> UpdateAsync(UserUpdateDto dto)
    {
        var exist = await unitOfWork.UserRepository.SelectAsync(d => d.Id == dto.Id)
            ?? throw new NotFoundException("User not found.");

        mapper.Map(dto, exist);

        await unitOfWork.UserRepository.UpdateAsync(exist);
        await unitOfWork.SaveAsync();

        return mapper.Map<UserResultDto>(exist);
    }

    public async Task<UserResultDto> UpdatePasswordAsync(long id, string oldPass, string newPass)
    {
        var user = await unitOfWork.UserRepository.SelectAsync(user =>
            user.Id == id) ?? throw new NotFoundException("User not found");

        if (!PasswordHasher.Verify(oldPass, user.Password))
            throw new InvalidOperationException("Old password is not correct!");

        user.Password = PasswordHasher.Hash(newPass);
        await unitOfWork.UserRepository.UpdateAsync(user);
        await unitOfWork.SaveAsync();

        return mapper.Map<UserResultDto>(user);
    }
}
