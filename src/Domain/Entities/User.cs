using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;

namespace Domain.Entities;

public class User
{
    public Guid Id { get; private set; }
    public string Email { get; private set; }
    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Password { get; private set; }
    
    public Wallet Wallet { get; private set; }

    private User() {}
    
    public static User Create(string email, string firstName, string lastName, string password)
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = email,
            FirstName = firstName,
            LastName = lastName,
            Wallet = new Wallet(),
        };

        var hashedPassword = new PasswordHasher<User>().HashPassword(user, password);

        user.Password = hashedPassword;
        
        return user;
    }
    
    public static User CreateWithWalletBalance(string email, string firstName, string lastName, string password, decimal balance)
    {
        var user = Create(email, firstName, lastName, password);
        
        user.Wallet.Deposit(balance);
        
        return user;
    }
}