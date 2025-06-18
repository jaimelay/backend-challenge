using CrossCutting.Interfaces.Repositories;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace CrossCutting.Repositories;

public class UserRepository(AppDbContext appDbContext) : IUserRepository
{
    public async Task<User?> GetByEmail(string email)
        => await appDbContext.Users.FirstOrDefaultAsync(user => user.Email == email);

    public async Task Save(User user)
    {
        await appDbContext.Users.AddAsync(user);
        await appDbContext.SaveChangesAsync();
    }    
}