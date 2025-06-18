using Application.Dtos.Requests;
using Application.Dtos.Responses;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IWalletAppService
{
    Task<IResult> GetBalance();
    Task<IResult> Deposit(WalletDepositRequest walletDepositRequest);
}