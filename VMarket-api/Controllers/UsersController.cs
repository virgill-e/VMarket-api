using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using VMarket_api.Data;
using VMarket_api.Models;
using VMarket_api.Models.DTOs;
using VMarket_api.Services;

namespace UserApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _db;

    public UsersController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpGet]
    public ActionResult<string> Get()
    {
        return Ok(_userService.GetHelloWorld());
    }
    
    
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto dto)
    {
        var result = await _userService.RegisterAsync(dto);

        if (!result.Success)
            return BadRequest(new { Errors = result.Errors });

        return Created($"api/users/{result.Information}", new { Id = result.Information });
    }
    
    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginDto dto)
    {
        var result = await _userService.LoginAsync(dto);
        if (!result.Success)
            return Unauthorized(new { Errors = result.Errors });

        return Ok(new 
        { 
            token = result.Information, 
            expiration = DateTime.UtcNow.AddMonths(1),
            username = dto.Email 
        });
    }
}