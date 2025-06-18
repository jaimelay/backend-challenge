using Domain.Entities;

namespace CrossCutting.Interfaces.Repositories;

public interface IWalletRepository
{
    Task<Wallet?> GetByUserId(Guid userId);
    Task<List<Wallet>> GetByUserIds(List<Guid> userIds);
    Task Update(Wallet wallet);
    Task UpdateRange(List<Wallet> wallets);
}