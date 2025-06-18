using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CrossCutting.Mappers;

public class TransferMapper : IEntityTypeConfiguration<Transfer>
{
    public void Configure(EntityTypeBuilder<Transfer> builder)
    {
        builder.ToTable("transfers");
        
        builder.HasKey(e => e.Id);

        builder.Property(e => e.Id);
        
        builder.Property(e => e.FromWalletId);
        
        builder.Property(e => e.ToWalletId);
        
        builder.Property(e => e.Amount).HasPrecision(18, 2);
        
        builder.Property(e => e.CreatedAt);

        builder.HasOne(e => e.FromWallet)
            .WithMany(e => e.TransfersSent)
            .HasForeignKey(e => e.FromWalletId);
        
        builder.HasOne(e => e.ToWallet)
            .WithMany(e => e.TransfersReceived)
            .HasForeignKey(e => e.ToWalletId);
    }
}