using IELTSBlog.Api.Models;
using IELTSBlog.Service.DTOs.Users;
using IELTSBlog.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Localization;

namespace IELTSBlog.Api.Controlers;

[ApiController]
[Route("api/[controller]/[action]")]
public class UsersController(IUserService userService) : ControllerBase
{
    [HttpPost]
    public async ValueTask<IActionResult> Create(UserCreationDto userCreationDto)
    {
        var result = await userService.CreateAsync(userCreationDto);

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
        var result = await userService.GetByIdAsync(id);

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
    public async ValueTask<IActionResult> GetAll()
    {
        var result = await userService.GetAllAsync();

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
    public async ValueTask<IActionResult> Update(UserUpdateDto userUpdateDto)
    {
        var result = await userService.UpdateAsync(userUpdateDto);

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
        var result = await userService.DeleteAsync(id);

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

