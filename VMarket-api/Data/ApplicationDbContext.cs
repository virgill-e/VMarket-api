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
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
        
    }
    public DbSet<Wallet> Wallets { get; set; } = null!;

    
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Wallet>()
            .HasOne(w => w.User)
            .WithOne() // 1 seul wallet par user
            .HasForeignKey<Wallet>(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade); // supprime wallet si user supprimé

        builder.Entity<Wallet>()
            .HasIndex(w => w.UserId);
    }
}