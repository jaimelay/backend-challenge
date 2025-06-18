using Domain.Entities;

namespace CrossCutting.Interfaces.Repositories;

public interface ITransferRepository
{
    Task<List<Transfer>> GetAllTransferByWalletIdAndDate(
        Guid walletId, 
        DateTime? fromDate = null,
        DateTime? toDate = null);
    Task Save(Transfer transfer);
}