using AutoMapper;
using IELTSBlog.Domain.Configrations;
using IELTSBlog.Domain.Entities;
using IELTSBlog.Repository.IRepositories;
using IELTSBlog.Service.DTOs.Categories;
using IELTSBlog.Service.Exceptions;
using IELTSBlog.Service.Interfaces;

namespace IELTSBlog.Service.Services;

public class CategoryService(
    IUnitOfWork unitOfWork,
    IMapper mapper) : ICategoryService
{
    public async Task<CategoryResultDto> CreateAsync(CategoryCreationDto dto)
    {
        var category = mapper.Map<Category>(dto);

        await unitOfWork.CategoryRepository.AddAsync(category);
        await unitOfWork.SaveAsync();

        return mapper.Map<CategoryResultDto>(category);
    }

    public async Task<bool> DeleteAsync(long id)
    {
        try
        {
            var category = await unitOfWork.CategoryRepository.SelectAsync(category =>
                category.Id == id)
                    ?? throw new NotFoundException($"Category not found with this Id - {id}");

            await unitOfWork.CategoryRepository.DeleteAsync(dbCategory => dbCategory == category);
            await unitOfWork.SaveAsync();

            return true;
        }
        catch (Exception exception)
        {
            return false;
        }
    }

    public async Task<IEnumerable<CategoryResultDto>> GetAllAsync(PaginationParams @params)
    {
        var categories = unitOfWork.CategoryRepository.SelectAll().AsEnumerable();
        return mapper.Map<IEnumerable<CategoryResultDto>>(categories);
    }

    public async Task<CategoryResultDto> GetByIdAsync(long id)
    {
        var category = await unitOfWork.CategoryRepository.SelectAsync(category =>
            category.Id == id) ?? throw new NotFoundException("Category not found.");

        return mapper.Map<CategoryResultDto>(category);
    }

    public async Task<CategoryResultDto> UpdateAsync(CategoryUpdateDto dto)
    {
        var existingCategory = await unitOfWork.CategoryRepository.SelectAsync(category =>
            category.Id == dto.Id)
                ?? throw new NotFoundException("Category not found.");

        existingCategory.Title = dto.Title;
        await unitOfWork.CategoryRepository.UpdateAsync(existingCategory);
        await unitOfWork.CategoryRepository.SaveAsync();

        return mapper.Map<CategoryResultDto>(existingCategory);
    }
}
