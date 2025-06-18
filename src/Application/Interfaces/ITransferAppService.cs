using Application.Dtos.Requests;
using Application.Dtos.Responses;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface ITransferAppService
{
    Task<IResult> CreateTransfer(CreateTransferRequest request);
    Task<IResult> GetTransfers(DateTime? fromDate, DateTime? toDate);
}