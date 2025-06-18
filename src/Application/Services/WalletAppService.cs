using Application.Dtos.Requests;
using Application.Dtos.Responses;
using Application.Interfaces;
using CrossCutting.Auth.Interfaces;
using CrossCutting.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;

namespace Application.Services;

public class WalletAppService(IWalletRepository walletRepository, IJwtProvider jwtProvider) : IWalletAppService
{
    public async Task<IResult> GetBalance()
    {
        var userId = jwtProvider.GetUserId();

        if (userId == Guid.Empty) Results.Problem("User not authenticated");

        var wallet = await walletRepository.GetByUserId(userId);
        
        if (wallet is null) return Results.Problem("Wallet not found");

        return Results.Ok(new WalletBallanceResponse { Balance = wallet.Balance });
    }

    public async Task<IResult> Deposit(WalletDepositRequest walletDepositRequest)
    {
        var userId = jwtProvider.GetUserId();

        if (userId == Guid.Empty) return Results.Problem("User not authenticated");
        
        var wallet = await walletRepository.GetByUserId(userId);
        
        if (wallet is null) return Results.Problem("Wallet not found");

        var oldBalance = wallet.Balance;
        
        wallet.Deposit(walletDepositRequest.Amount);
        
        await walletRepository.Update(wallet);
        
        return Results.Ok(new WalletDepositResponse { OldBalance = oldBalance, NewBalance = wallet.Balance });
    }
}