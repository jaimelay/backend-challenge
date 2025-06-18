using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrossCutting.Mappers;

public class WalletMapper : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.ToTable("wallets");

        builder.HasKey(e => e.Id);
        
        builder.Property(e => e.Id);
        
        builder.Property(e => e.UserId);
        
        builder.Property(e => e.Balance).HasPrecision(18, 2);
        
        builder.HasOne(e => e.User)
            .WithOne(e => e.Wallet)
            .HasForeignKey<Wallet>(e => e.UserId)
            .IsRequired();
    }
}