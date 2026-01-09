using VMarket_api.Models.DTOs;

namespace VMarket_api.Services;

public interface IUserService
{
    string GetHelloWorld();
    Task<ServiceResult> RegisterAsync(RegisterDto dto);
    
}