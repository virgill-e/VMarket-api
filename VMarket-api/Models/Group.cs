using VMarket_api.Data;

namespace VMarket_api.Models;

public class Group
{
    public string Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string ImagePath { get; set; } = string.Empty; // "groups/abc123.png"
    public string OwnerId { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<GroupMembership> Members { get; set; } = new List<GroupMembership>();

    public ApplicationUser User { get; set; } = null!;
}
