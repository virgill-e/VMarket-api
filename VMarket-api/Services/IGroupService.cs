using VMarket_api.Models.DTOs;

namespace VMarket_api.Services;

public interface IGroupService
{
    Task<ServiceResult> CreateGroupAsync(string userId, CreateGroupDto dto);
}