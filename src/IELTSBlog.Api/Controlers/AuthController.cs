using IELTSBlog.Api.Models;
using IELTSBlog.Service.DTOs.Users;
using IELTSBlog.Service.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IELTSBlog.Api.Controlers;


[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    [HttpPost("login")]
    public async ValueTask<IActionResult> LoginAsync(LoginModel loginModel)
    {
        var result = await authService.LoginAsync(loginModel.Email, loginModel.Password);

        return result is not null
            ? Ok(new Response
            {
                StatusCode = 200,
                Message = "Login successful",
                Data = result
            })
            : BadRequest(new Response
            {
                StatusCode = 401,
                Message = "Invalid email or password"
            });
    }

    [HttpPost("register")]
    public async Task<IActionResult> RegisterAsync(UserCreationDto registerDto)
    {
        await authService.RegisterAsync(registerDto);
        await authService.SendCodeForRegisterAsync($"{registerDto.Email}");
        var serviceResult = await authService.VerifyRegisterAsync(registerDto.Email, 0000);
        return Ok(new Response()
        {
            StatusCode = 200,
            Message = "Registrated successfully",
            Data = serviceResult.Token
        });
    }

    [HttpPost("register/verify")]
    public async Task<IActionResult> VerifyRegisterAsync(VerifyModel model)
    {
        var serviceResult = await authService.VerifyRegisterAsync(model.Email, int.Parse(model.Code));
        if (serviceResult.Result == false)
        {
            return BadRequest(new Response()
            {
                StatusCode = 401,
                Message = "Invalid code"
            });
        }
        return Ok(new Response()
        {
            StatusCode = 200,
            Message = "User verfied successfully",
            Data = serviceResult.Token
        });
    }
}
