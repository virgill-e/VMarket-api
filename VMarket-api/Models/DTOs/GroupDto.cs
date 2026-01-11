namespace VMarket_api.Models.DTOs;

public class GroupDto
{
    public string Id { get; set; }
    
    public string Name { get; set; }
    
    public string ImagePath { get; set; }
    
    public int NumberOfMembers { get; set; }
    
    public float Balance { get; set; }
}