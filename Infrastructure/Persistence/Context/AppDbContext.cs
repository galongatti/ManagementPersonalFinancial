using Domain.Entities;
using Infrastructure.Persistence.Context.EntitiesConfiguration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Context;

public class AppDbContext : IdentityDbContext<IdentityUser>
{
    
    public DbSet<FamilyGroup> FamilyGroups { get; set; }
    public DbSet<User> Users { get; set; }
    
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        builder.ApplyConfiguration(new UserConfiguration());
        builder.ApplyConfiguration(new FamilyGroupConfiguration());
    }
    

}