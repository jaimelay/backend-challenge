using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using CrossCutting.Auth.Interfaces;
using Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace CrossCutting.Auth;

public class JwtProvider(IConfiguration configuration, IHttpContextAccessor httpContextAccessor) : IJwtProvider
{
    public string GenerateToken(User user)
    {
        var claims = new ClaimsIdentity([
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString())
        ]);

        var signingCredentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["Jwt:SecretKey"])), 
            SecurityAlgorithms.HmacSha256);

        var token = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddMinutes(configuration.GetValue<int>("Jwt:TokenValidityInMinutes")),
            Issuer = configuration["Jwt:Issuer"],
            Audience = configuration["Jwt:Audience"],
            SigningCredentials = signingCredentials
        };
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var securityToken = tokenHandler.CreateToken(token);
        var accessToken = tokenHandler.WriteToken(securityToken);
        
        return accessToken;
    }

    public Guid GetUserId()
    {
        if (httpContextAccessor.HttpContext is null) return Guid.Empty;
        
        var userIdClaim = httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(e => e.Type == JwtRegisteredClaimNames.Sub)?.Value;
        
        return Guid.TryParse(userIdClaim, out var userId) ? userId : Guid.Empty;
    }
}