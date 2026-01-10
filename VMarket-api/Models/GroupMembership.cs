using VMarket_api.Data;

namespace VMarket_api.Models;

public class GroupMembership
{
    public string GroupId { get; set; } = string.Empty;
    public string UserId { get; set; } = string.Empty;
    public DateTime JoinedAt { get; set; } = DateTime.UtcNow;
    public bool IsAdmin { get; set; } = false; // privilèges

    // Navigation properties
    public Group Group { get; set; } = null!;
    public ApplicationUser User { get; set; } = null!;
}
