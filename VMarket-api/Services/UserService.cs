using Microsoft.AspNetCore.Identity;
using VMarket_api.Data;
using VMarket_api.Models;
using VMarket_api.Models.DTOs;

namespace VMarket_api.Services;

public class UserService : IUserService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _db;
    
    public UserService(UserManager<ApplicationUser> userManager, ApplicationDbContext db)
    {
        _userManager = userManager;
        _db = db;
    }
    
    public string GetHelloWorld() => "HELLO WORLD";
    
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
            // Créer wallet auto
            var wallet = new Wallet { UserId = user.Id };
            _db.Wallets.Add(wallet);
            await _db.SaveChangesAsync();

            return new(true, user.UserName);
        }

        return new(false, null, result.Errors.Select(e => e.Description).ToArray());
    }
    
}