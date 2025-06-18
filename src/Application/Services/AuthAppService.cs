using Application.Dtos.Requests;
using Application.Dtos.Responses;
using Application.Interfaces;
using CrossCutting;
using CrossCutting.Auth.Interfaces;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Application.Services;

public class AuthAppService(AppDbContext dbContext, IJwtProvider jwtProvider, IValidator<UserRegisterRequest> userRegisterValidator) : IAuthAppService
{
    public async Task<IResult> Register(UserRegisterRequest request)
    {
        var validation = await userRegisterValidator.ValidateAsync(request);
        
        if (!validation.IsValid)
            return Results.ValidationProblem(validation.ToDictionary());
        
        var user = await dbContext.Users.FirstOrDefaultAsync(user => user.Email == request.Email);

        if (user is not null) 
            return Results.Problem("User is already registered.");
        
        var newUser = User.Create(request.Email, request.FirstName, request.LastName, request.Password);
        
        await dbContext.Users.AddAsync(newUser);
        await dbContext.SaveChangesAsync();

        return Results.Ok();
    }
    
    public async Task<IResult> Login(UserLoginRequest request)
    {
        var user = await dbContext.Users.FirstOrDefaultAsync(user => user.Email == request.Email);
        
        if (user is null) return Results.Problem("User is not registered.");
        
        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, request.Password) == PasswordVerificationResult.Failed)
            return Results.Problem("Password is incorrect.");
            

        return Results.Ok(new UserLoginResponse { AccessToken = jwtProvider.GenerateToken(user) });
    }
}