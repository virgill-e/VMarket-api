using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using VMarket_api.Models;

namespace VMarket_api.Data;

public class ApplicationUser : IdentityUser
{
    // Tu pourras ajouter des props custom ici plus tard (FirstName, etc.)
}

public class ApplicationDbContext 
    : IdentityDbContext<ApplicationUser, IdentityRole, string>
{
    public DbSet<Group> Groups { get; set; } = null!;
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
    }
}