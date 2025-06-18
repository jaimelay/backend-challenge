using Application.Dtos.Requests;
using Application.Dtos.Responses;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace desafioBackend.Endpoints;

public static class WalletEndpoints
{
    public static IEndpointRouteBuilder UseWalletEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var walletEndpoints = endpoints.MapGroup("/wallet");

        walletEndpoints.MapGet("/balance",
            async (IWalletAppService walletAppService) => await walletAppService.GetBalance())
            .Produces<WalletBallanceResponse>(StatusCodes.Status200OK)
            .RequireAuthorization();
        
        walletEndpoints.MapPost("/deposit", 
                async (IWalletAppService walletAppService, [FromBody] WalletDepositRequest walletDepositRequest) => await walletAppService.Deposit(walletDepositRequest))
            .Produces<WalletDepositResponse>(StatusCodes.Status200OK)
            .RequireAuthorization();
        
        return walletEndpoints;
    }
}