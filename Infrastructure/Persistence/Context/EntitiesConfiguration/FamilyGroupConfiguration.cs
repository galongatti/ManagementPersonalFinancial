using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Context.EntitiesConfiguration;

public class FamilyGroupConfiguration : IEntityTypeConfiguration<FamilyGroup>
{
    public void Configure(EntityTypeBuilder<FamilyGroup> builder)
    {
        
        builder.HasKey(a => a.Id);
        builder.Property(f => f.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(f => f.DateCreated);

        builder.Property(f => f.DateModified);

        builder.HasMany(f => f.Members).WithOne(u => u.FamilyGroup).HasForeignKey(u => u.FamilyGroupId);
        builder.HasOne(f => f.Owner).WithOne(c => c.FamilyGroupOwner).HasForeignKey<User>(u => u.FamilyGroupOwnerId);
    }
}