using IELTSBlog.Api.Models;
using IELTSBlog.Domain.Configrations;
using IELTSBlog.Service.DTOs.Categories;
using IELTSBlog.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IELTSBlog.Api.Controlers;

[ApiController]
[Route("api/[controller]/[action]")]
public class CategoriesController(ICategoryService categoryService) : ControllerBase
{
    [HttpPost]
    public async ValueTask<IActionResult> Create(CategoryCreationDto categoryCreationDto)
    {
        var result = await categoryService.CreateAsync(categoryCreationDto);

        return result is not null
            ? Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = result
            })
            : BadRequest(new Response
            {
                StatusCode = 400,
                Message = "Invalid information"
            });
    }

    [HttpGet]
    public async ValueTask<IActionResult> Get(long id)
    {
        var result = await categoryService.GetByIdAsync(id);

        return result is not null
            ? Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = result
            })
            : BadRequest(new Response
            {
                StatusCode = 400,
                Message = "Invalid information"
            });
    }

    [HttpGet]
    public async ValueTask<IActionResult> GetAll([FromQuery] PaginationParams @params)
    {
        var result = await categoryService.GetAllAsync(@params);

        return result is not null
            ? Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = result
            })
            : BadRequest(new Response
            {
                StatusCode = 400,
                Message = "Invalid information"
            });
    }

    [HttpPut]
    public async ValueTask<IActionResult> Update(CategoryUpdateDto categoryUpdate)
    {
        var result = await categoryService.UpdateAsync(categoryUpdate);

        return result is not null
            ? Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
                Data = result
            })
            : BadRequest(new Response
            {
                StatusCode = 400,
                Message = "Invalid information"
            });
    }

    [HttpDelete]
    public async ValueTask<IActionResult> Delete(long id)
    {
        var result = await categoryService.DeleteAsync(id);

        return result
            ? Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
            })
            : BadRequest(new Response
            {
                StatusCode = 400,
                Message = "Invalid information"
            });
    }
}
