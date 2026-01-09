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

    public UsersController(IUserService userService, UserManager<ApplicationUser> userManager, ApplicationDbContext db)
    {
        _userService = userService;
        _userManager = userManager;
        _db = db;
    }

    [HttpGet]
    public ActionResult<string> Get()
    {
        return Ok(_userService.GetHelloWorld());
    }
    
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto dto)
    {
        //TODO: mettre une partie dans les services !!!
        
        if (await _userManager.FindByEmailAsync(dto.Email) != null)
            return BadRequest("Email déjà utilisé");
        
        if(dto.Password != dto.ConfirmPassword)
            return BadRequest("Les mots de passe ne correspondent pas");

        var user = new ApplicationUser
        {
            UserName = dto.Username,
            Email = dto.Email,
            EmailConfirmed = true // TODO: temporaire sinon faut gérer la confirmation par email
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (result.Succeeded)
        {
            var wallet = new Wallet { UserId = user.Id };
            _db.Wallets.Add(wallet);
            await _db.SaveChangesAsync();

            return Created($"api/users/{user.Id}", new { Id = user.Id, Username = user.UserName });
        }

        return BadRequest(new { Errors = result.Errors.Select(e => e.Description) });
    }
}