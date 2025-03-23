using CustomerManagement.Application.DTOs;
using CustomerManagement.Application.Interfaces.Services;
using Microsoft.AspNetCore.Mvc;

namespace CustomerManagement.Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequestDto request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var token = _authService.Login(request.Email, request.Password);
        if (token == null)
            return Unauthorized(new { message = "Invalid email or password" });

        return Ok(new { token });
    }
}
