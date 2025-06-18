using Application.Dtos.Requests;
using Application.Dtos.Responses;
using Application.Interfaces;
using CrossCutting;
using CrossCutting.Auth.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class TransferAppService(AppDbContext appDbContext, IJwtProvider jwtProvider) : ITransferAppService
{
    public async Task<IResult> CreateTransfer(CreateTransferRequest request)
    {
        var userId = jwtProvider.GetUserId();

        if (userId == Guid.Empty) return Results.Problem("User not authenticated");
        
        if (userId == request.ToUserId) return Results.Problem("It's not possible to do the transfer to the same account.");
        
        var wallets = await appDbContext.Wallets.Where(e => e.UserId == userId || e.UserId == request.ToUserId).ToListAsync();
        
        if (wallets.Count == 0 || wallets is null) return Results.Problem("No wallets found");
        
        var walletFromUser = wallets.First(e => e.UserId == userId);

        if (request.Amount > walletFromUser.Balance) return Results.Problem("Insufficient balance"); 
        
        var walletToUser = wallets.First(e => e.UserId == request.ToUserId);
        
        walletFromUser.Withdraw(request.Amount);
        walletToUser.Deposit(request.Amount);

        var newTransfer = Transfer.Create(walletFromUser.Id, walletToUser.Id, request.Amount);
        
        appDbContext.Wallets.UpdateRange([ walletFromUser, walletToUser ]);
        await appDbContext.Transfers.AddAsync(newTransfer);
        await appDbContext.SaveChangesAsync();

        return Results.Ok();
    }

    public async Task<IResult> GetTransfers(DateTime? fromDate, DateTime? toDate)
    {
        var userId = jwtProvider.GetUserId();

        if (userId == Guid.Empty) return Results.Problem("User not authenticated");

        var wallet = await appDbContext.Wallets.FirstOrDefaultAsync(e => e.UserId == userId);
        
        if (wallet is null) return Results.Problem("No wallet found");
        
        var query = appDbContext.Transfers.Where(e => e.FromWalletId == wallet.Id || e.ToWalletId == wallet.Id).AsNoTracking();

        if (fromDate.HasValue)
            query = query.Where(t => t.CreatedAt >= fromDate).AsNoTracking();
        
        if (toDate.HasValue)
            query = query.Where(t => t.CreatedAt <= toDate).AsNoTracking();
        
        var transfers = await query.AsNoTracking().ToListAsync();
        
        return Results.Ok(new GetTransfersResponse
        {
            ReceivedTransfers = transfers.Where(e => e.ToWalletId == wallet.Id).ToList(), 
            SentTransfers = transfers.Where(e => e.FromWalletId == wallet.Id).ToList()
        });
    }
}