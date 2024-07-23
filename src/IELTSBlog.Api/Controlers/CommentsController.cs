using IELTSBlog.Api.Models;
using IELTSBlog.Domain.Configrations;
using IELTSBlog.Service.DTOs.Comments;
using IELTSBlog.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IELTSBlog.Api.Controlers;

[ApiController]
[Route("api/[controller]/[action]")]
[Authorize]
public class CommentsController(ICommentService commentService) : Controller
{
    [HttpPost]
    public async ValueTask<IActionResult> Create(CommentCreationDto commentCreationDto)
    {
        var result = await commentService.CreateAsync(commentCreationDto);

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
    public async ValueTask<IActionResult> Get(long id)
    {
        var result = await commentService.GetByIdAsync(id);

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
    public async ValueTask<IActionResult> GetAll([FromQuery] PaginationParams pagination)
    {
        var result = await commentService.GetAllAsync(pagination);

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
    public async ValueTask<IActionResult> Update(CommentUpdateDto commentUpdateDto)
    {
        var result = await commentService.UpdateAsync(commentUpdateDto);

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
        var result = await commentService.DeleteAsync(id);

        return result
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
