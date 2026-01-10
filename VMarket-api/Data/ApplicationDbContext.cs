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
}