using Domain.Entities;

namespace Application.Dtos.Responses;

public class GetTransfersResponse
{
    public List<Transfer> ReceivedTransfers { get; set; }
    public List<Transfer> SentTransfers { get; set; }
}