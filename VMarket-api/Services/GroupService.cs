using VMarket_api.Data;
using VMarket_api.Models;
using VMarket_api.Models.DTOs;

namespace VMarket_api.Services;

public class GroupService: IGroupService
{
    
    private readonly ApplicationDbContext _db;
    public GroupService(ApplicationDbContext db)
    {
        _db = db;
    }
    
    public async Task<ServiceResult> CreateGroupAsync(string userId, CreateGroupDto dto)
    {
        // Valid image + size (ton code)
        if (!IsValidImage(dto.Image))
            return new(false, null, new[] { "Image invalide (PNG/JPEG/GIF seulement)" });

        if (dto.Image.Length > 10 * 1024 * 1024)
            return new(false, null, new[] { "Image trop lourde (max 10Mo)" });

        var fileName = $"{Guid.NewGuid()}.{(dto.Image.ContentType.Split('/')?[1] ?? "png")}";
        var path = Path.Combine("wwwroot", "images", "groups", fileName);

        // Créer dossier si absent
        Directory.CreateDirectory(Path.GetDirectoryName(path)!);

        // Sauvegarder fichier
        using (var stream = new FileStream(path, FileMode.Create))
        {
            await dto.Image.CopyToAsync(stream);
        }

        var group = new Group 
        { 
            Name = dto.Name,
            ImagePath = $"images/groups/{fileName}",
            OwnerId = userId 
        };
        _db.Groups.Add(group);
        await _db.SaveChangesAsync();
        
        var membership = new GroupMembership 
        { 
            GroupId = group.Id, 
            UserId = userId, 
            IsAdmin = true
            
        };

        _db.GroupMemberships.Add(membership);
        await _db.SaveChangesAsync();
        
        

        return new(true , null , null);
    }

    public Task<ServiceResult> GetGroupsAsync(string userId)
    {
        var groups = _db.Groups
            .Where(g => g.OwnerId == userId)
            .Select(g => new GroupDto 
            { 
                Id = g.Id,
                Name = g.Name,
                ImagePath = g.ImagePath,
                NumberOfMembers = g.Members.Count,
                Balance = g.Members
                    .Where(m => m.UserId == userId)
                    .Select(m => m.Balance)
                    .FirstOrDefault()
            })
            .ToList();
        
        return Task.FromResult(new ServiceResult(true, groups, null));
    }
    
    public Task<ServiceResult> GetGroupByIdAsync(string userId, string groupId)
    {
        if(!_db.GroupMemberships.Any(gm => gm.GroupId == groupId && gm.UserId == userId))
            return Task.FromResult(new ServiceResult(false, null, new[] { "Accès refusé au groupe." }));
        
        var group = _db.Groups
            .Where(g => g.Id == groupId)
            .Select(g => new GroupDto 
            { 
                Id = g.Id,
                Name = g.Name,
                ImagePath = g.ImagePath,
                NumberOfMembers = g.Members.Count,
                Balance = g.Members
                    .Where(m => m.UserId == userId)
                    .Select(m => m.Balance)
                    .FirstOrDefault()
            })
            .FirstOrDefault();
        
        if (group == null)
            return Task.FromResult(new ServiceResult(false, null, new[] { "Groupe non trouvé ou accès refusé." }));
        
        return Task.FromResult(new ServiceResult(true, group, null));
    }

    private static bool IsValidImage(IFormFile file)
    {
        var allowed = new[] { "image/png", "image/jpeg", "image/gif" };
        return allowed.Contains(file.ContentType.ToLower());
    }
}