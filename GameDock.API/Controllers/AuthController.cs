using GameDock.BLL.Containers;
using GameDock.DTO.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace GameDock.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AuthContainer _authContainer;

    public AuthController(AuthContainer authContainer)
    {
        _authContainer = authContainer;
    }
    
    [AllowAnonymous]
    [HttpPost("register")]
    public ActionResult<AuthResponseDto> Register(RegisterRequestDto request)
    {
        try
        {
            var result = _authContainer.Register(request);
            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [AllowAnonymous]
    [HttpPost("login")]
    public ActionResult<AuthResponseDto> Login(LoginRequestDto request)
    {
        try
        {
            var result = _authContainer.Login(request);
            return Ok(result);
        }
        catch (UnauthorizedAccessException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}