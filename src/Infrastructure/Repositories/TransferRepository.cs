using CrossCutting.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrossCutting.Repositories;

public class TransferRepository(AppDbContext appDbContext) : ITransferRepository
{
    public async Task<List<Transfer>> GetAllTransferByWalletIdAndDate(Guid walletId, DateTime? fromDate = null, DateTime? toDate = null)
    {
        var query = appDbContext.Transfers.Where(e => e.FromWalletId == walletId || e.ToWalletId == walletId);

        if (fromDate.HasValue)
            query = query.Where(t => t.CreatedAt.Date >= fromDate.Value.Date);
        
        if (toDate.HasValue)
            query = query.Where(t => t.CreatedAt.Date <= toDate.Value.Date);
        
        return await query.ToListAsync();
    }
    
    public async Task Save(Transfer transfer)
    {
        await appDbContext.Transfers.AddAsync(transfer);
        await appDbContext.SaveChangesAsync();
    }
}