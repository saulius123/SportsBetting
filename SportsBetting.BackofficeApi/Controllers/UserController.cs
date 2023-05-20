using Microsoft.AspNetCore.Mvc;
using SportsBetting.Services.Services.Interfaces;
using SportsBetting.Services.DTOs;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public UserController(IIdentityService identityService)
    {
        _identityService = identityService;
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateUserDto createUserDto)
    {
        var creationResult = await _identityService.Create(createUserDto.Username, createUserDto.Password, createUserDto.Email);

        if (!creationResult)
        {
            return BadRequest(new { message = "Username already exists." });
        }

        return Ok(new { message = "User created successfully." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(LoginDto loginDto)
    {
        var token = await _identityService.Login(loginDto.Username, loginDto.Password);

        if (token == null)
        {
            return Unauthorized(new { message = "Invalid username or password." });
        }

        return Ok(new { token });
    }
}