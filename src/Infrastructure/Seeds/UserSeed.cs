using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CrossCutting.Seeds;

public static class UserSeed
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using var context = new AppDbContext(serviceProvider.GetRequiredService<DbContextOptions<AppDbContext>>());

        if (context.Users.Any()) return;

        var user1 = User.CreateWithWalletBalance("alice@email.com", "Alice", "Silva", "password", 200);
        var user2 = User.CreateWithWalletBalance("bob@email.com", "Bob", "Vieira", "password", 150);
        var user3 = User.CreateWithWalletBalance("john@email.com", "John", "Doe", "password", 100);

        context.Users.AddRange(user1, user2, user3);
        context.SaveChanges();
    }
}