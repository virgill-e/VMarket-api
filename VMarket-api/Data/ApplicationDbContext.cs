using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VMarket_api.Models;

namespace VMarket_api.Data;

public class ApplicationUser : IdentityUser
{
}

public class ApplicationDbContext 
    : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public DbSet<Group> Groups { get; set; } = null!;
    public DbSet<GroupMembership> GroupMemberships { get; set; } = null!;
    
    
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }
    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder); // Identity d'abord !

        // Group : Id auto-généré SQLite
        builder.Entity<Group>(entity =>
        {
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd(); // ← ÇA !

            // Tes configs FK wallet, etc.
        });
        
        builder.Entity<GroupMembership>()
            .HasKey(gm => new { gm.GroupId, gm.UserId }); // Composite PK

        builder.Entity<GroupMembership>()
            .HasOne(gm => gm.Group)
            .WithMany(g => g.Members)
            .HasForeignKey(gm => gm.GroupId);

        builder.Entity<GroupMembership>()
            .HasOne(gm => gm.User)
            .WithMany()
            .HasForeignKey(gm => gm.UserId);

    }
}