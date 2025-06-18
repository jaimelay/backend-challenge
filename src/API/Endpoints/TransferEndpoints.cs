using Application.Dtos.Requests;
using Application.Dtos.Responses;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace desafioBackend.Endpoints;

public static class TransferEndpoints
{
    public static IEndpointRouteBuilder UseTransferEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var transferEndpoints = endpoints.MapGroup("/transfer");

        transferEndpoints.MapPost("/", async (ITransferAppService transferAppService, [FromBody] CreateTransferRequest createTransferRequest) 
                => await transferAppService.CreateTransfer(createTransferRequest))
            .Produces<IResult>()
            .RequireAuthorization();

        transferEndpoints.MapGet("/", async (ITransferAppService transferAppService, [FromQuery] DateTime? fromDate, [FromQuery] DateTime? toDate) => await transferAppService.GetTransfers(fromDate, toDate))
            .Produces<IResult>()
            .RequireAuthorization();
        
        return transferEndpoints;
    }
}