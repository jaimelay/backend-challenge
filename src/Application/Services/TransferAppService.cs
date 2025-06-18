using Application.Dtos.Requests;
using Application.Dtos.Responses;
using Application.Interfaces;
using CrossCutting.Auth.Interfaces;
using CrossCutting.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class TransferAppService(ITransferRepository transferRepository, IWalletRepository walletRepository, IJwtProvider jwtProvider) : ITransferAppService
{
    public async Task<IResult> CreateTransfer(CreateTransferRequest request)
    {
        var userId = jwtProvider.GetUserId();

        if (userId == Guid.Empty) return Results.Problem("User not authenticated");
        
        if (userId == request.ToUserId) return Results.Problem("It's not possible to do the transfer to the same account.");
        
        var wallets = await walletRepository.GetByUserIds([userId, request.ToUserId]);
        
        if (wallets.Count == 0 || wallets is null) return Results.Problem("No wallets found");
        
        var walletFromUser = wallets.First(e => e.UserId == userId);

        if (request.Amount > walletFromUser.Balance) return Results.Problem("Insufficient balance"); 
        
        var walletToUser = wallets.First(e => e.UserId == request.ToUserId);
        
        walletFromUser.Withdraw(request.Amount);
        walletToUser.Deposit(request.Amount);

        var newTransfer = Transfer.Create(walletFromUser.Id, walletToUser.Id, request.Amount);
        
        await walletRepository.UpdateRange([ walletFromUser, walletToUser ]);

        await transferRepository.Save(newTransfer);

        return Results.Ok();
    }

    public async Task<IResult> GetTransfers(DateTime? fromDate, DateTime? toDate)
    {
        var userId = jwtProvider.GetUserId();

        if (userId == Guid.Empty) return Results.Problem("User not authenticated");

        var wallet = await walletRepository.GetByUserId(userId);
        
        if (wallet is null) return Results.Problem("No wallet found");

        var transfers = await transferRepository.GetAllTransferByWalletIdAndDate(wallet.Id, fromDate, toDate);
        
        return Results.Ok(new GetTransfersResponse
        {
            ReceivedTransfers = transfers.Where(e => e.ToWalletId == wallet.Id).ToList(), 
            SentTransfers = transfers.Where(e => e.FromWalletId == wallet.Id).ToList()
        });
    }
}