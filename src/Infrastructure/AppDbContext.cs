using CrossCutting.Mappers;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrossCutting;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Wallet> Wallets { get; set; }
    public DbSet<Transfer> Transfers { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.ApplyConfiguration(new TransferMapper());
        modelBuilder.ApplyConfiguration(new UserMapper());
        modelBuilder.ApplyConfiguration(new WalletMapper());
    }
}