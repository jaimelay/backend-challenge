namespace Domain.Entities;

public class Transfer
{
    public Guid Id { get; private set; }
    public Guid FromWalletId { get; private set; }
    public Guid ToWalletId { get; private set; }
    public decimal Amount { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public virtual Wallet FromWallet { get; private set; }
    public virtual Wallet ToWallet { get; private set; }

    private Transfer() {}
    
    public static Transfer Create(Guid fromWalletId, Guid toWalletId, decimal amount)
    {
        return new Transfer
        {
            Id = Guid.NewGuid(),
            FromWalletId = fromWalletId,
            ToWalletId = toWalletId,
            Amount = amount,
            CreatedAt = DateTime.UtcNow
        };
    }
}