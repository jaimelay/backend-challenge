namespace Application.Dtos.Requests;

public class CreateTransferRequest
{
    public Guid ToUserId { get; set; }
    public decimal Amount { get; set; }
}