using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using VMarket_api.Data;
using VMarket_api.Models;
using VMarket_api.Models.DTOs;

namespace VMarket_api.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _db;
    private readonly IConfiguration _config;
    
    public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext db, IConfiguration config)
    {
        _userManager = userManager;
        _db = db;
        _config = config;
    }
    public async Task<ServiceResult> RegisterAsync(RegisterDto dto)
    {
        // Vérif email existant
        if (await _userManager.FindByEmailAsync(dto.Email) != null)
            return new(false, null, new[] { "Email déjà utilisé" });
        
        if(dto.Password != dto.ConfirmPassword)
            return new(false, null,new[] { "Les mots de passe ne correspondent pas" });

        var user = new ApplicationUser
        {
            UserName = dto.Username,
            Email = dto.Email,
            EmailConfirmed = true //TODO: dans le futur une confirmation par mail
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (result.Succeeded)
        {
            return new(true, user.UserName);
        }

        return new(false, null, result.Errors.Select(e => e.Description).ToArray());
    }
    
    public async Task<ServiceResult> LoginAsync(LoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            return new ServiceResult(false, null, new[] { "Identifiants invalides" });

        var claims = new[]
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id),
            new Claim(ClaimTypes.Name, user.UserName!),
            new Claim(ClaimTypes.Email, user.Email!)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMonths(1),
            signingCredentials: creds
        );

        var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
        return new ServiceResult(true, tokenString);
    }


    public async Task<ServiceResult> GetCurrentUserAsync(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return new(false, null, new[] { "User non trouvé" });
        
        var profile = new UserProfileDto
        {
            Username = user.UserName ?? "",
            Email = user.Email ?? "",
        };

        return new(true, profile);
    }
}
