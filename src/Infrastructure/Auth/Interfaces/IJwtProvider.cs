using Domain.Entities;

namespace CrossCutting.Auth.Interfaces;

public interface IJwtProvider
{
    string GenerateToken(User user);
    Guid GetUserId();
}