using Domain.Entities;

namespace CrossCutting.Interfaces.Repositories;

public interface IUserRepository
{
    Task<User?> GetByEmail(string email);
    Task Save(User user);
}