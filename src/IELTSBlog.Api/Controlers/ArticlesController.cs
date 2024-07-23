using IELTSBlog.Api.Models;
using IELTSBlog.Domain.Configrations;
using IELTSBlog.Service.DTOs.Articles;
using IELTSBlog.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IELTSBlog.Api.Controlers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class ArticlesController(IArticleService articleService) : ControllerBase
{
    [HttpDelete]
    public async ValueTask<IActionResult> Delete(long id)
    {
        var result = await articleService.DeleteAsync(id);

        return result
            ? Ok(new Response
            {
                StatusCode = 200,
                Message = "Success",
            })
            : BadRequest(new Response
            {
                StatusCode = 400,
                Message = "Invalid data"
            });
    }

    [HttpGet]
    [AllowAnonymous]
    public async ValueTask<IActionResult> Get(long id)
    {
        var result = await articleService.GetByIdAsync(id);

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
    [AllowAnonymous]
    public async ValueTask<IActionResult> GetAll([FromQuery] PaginationParams paginationParams)
    {
        var result = await articleService.GetAllAsync(paginationParams);

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

    [HttpPost]
    public async ValueTask<IActionResult> Create(ArticleCreationDto articleCreationDto)
    {
        var result = await articleService.CreateAsync(articleCreationDto);

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
    public async ValueTask<IActionResult> Update(ArticleUpdateDto articleUpdateDto)
    {
        var result = await articleService.UpdateAsync(articleUpdateDto);

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

}
