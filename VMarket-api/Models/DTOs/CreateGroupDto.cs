namespace VMarket_api.Models.DTOs;

public class CreateGroupDto
{
    public string Name { get; set; } = string.Empty;
    
    public IFormFile Image { get; set; } = null!;
}