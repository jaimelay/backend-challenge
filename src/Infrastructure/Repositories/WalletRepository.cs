using CrossCutting.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrossCutting.Repositories;

public class WalletRepository(AppDbContext appDbContext) : IWalletRepository
{
    public async Task<Wallet?> GetByUserId(Guid userId)
        => await appDbContext.Wallets.FirstOrDefaultAsync(e => e.UserId == userId);

    public async Task<List<Wallet>> GetByUserIds(List<Guid> userIds)
        => await appDbContext.Wallets.Where(e => userIds.Contains(e.UserId)).ToListAsync();
    
    public async Task Update(Wallet wallet)
    {
        appDbContext.Wallets.Update(wallet);
        await appDbContext.SaveChangesAsync();
    }
    
    public async Task UpdateRange(List<Wallet> wallets)
    {
        appDbContext.Wallets.UpdateRange(wallets);
        await appDbContext.SaveChangesAsync();
    }
}