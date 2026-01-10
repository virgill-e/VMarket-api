using VMarket_api.Models.DTOs;

namespace VMarket_api.Services;

public interface IUserService
{
    Task<ServiceResult> RegisterAsync(RegisterDto dto);
    
    Task<ServiceResult> LoginAsync(LoginDto dto);
    
    Task<ServiceResult> GetCurrentUserAsync(string userId);
}