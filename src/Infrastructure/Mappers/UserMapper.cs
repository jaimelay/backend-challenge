using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrossCutting.Mappers;

public class UserMapper : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable("users");

        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id);
        
        builder.Property(e => e.Email).HasMaxLength(100);

        builder.Property(e => e.FirstName).HasMaxLength(100);
        
        builder.Property(e => e.LastName).HasMaxLength(100);

        builder.HasIndex(e => e.Email).IsUnique();
    }
}