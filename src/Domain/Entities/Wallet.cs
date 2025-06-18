using System.Text.Json.Serialization;

namespace Domain.Entities;

public class Wallet
{
    public Guid Id { get; private set; }
    public Guid UserId { get; private set; }
    public decimal Balance { get; private set; }
    public User User { get; }
    [JsonIgnore] public ICollection<Transfer> TransfersSent { get; private set; }
    [JsonIgnore] public ICollection<Transfer> TransfersReceived { get; private set; }

    public void Deposit(decimal amount)
    {
        if (amount <= 0) throw new Exception("Invalid amount.");
        
        Balance += amount;
    }

    public void Withdraw(decimal amount)
    {
        if (amount <= 0) throw new Exception("Invalid amount.");
        
        Balance -= amount;
    }
}