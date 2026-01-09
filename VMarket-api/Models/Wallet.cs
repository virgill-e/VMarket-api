using VMarket_api.Data;

namespace VMarket_api.Models;

public class Wallet
{
    public string Id { get; set; } = Guid.NewGuid().ToString(); // ou int si tu préfères
    public string UserId { get; set; } = string.Empty; // FK vers IdentityUser.Id (string)
    public decimal Balance { get; set; } = 0;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public ApplicationUser? User { get; set; } // optionnel, référence inverse

}