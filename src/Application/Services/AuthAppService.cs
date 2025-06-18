using Application.Dtos.Requests;
using Application.Dtos.Responses;
using Application.Interfaces;
using CrossCutting.Auth.Interfaces;
using CrossCutting.Interfaces.Repositories;
using Domain.Entities;
using FluentValidation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Application.Services;

public class AuthAppService(IUserRepository userRepository, IJwtProvider jwtProvider, IValidator<UserRegisterRequest> userRegisterValidator) : IAuthAppService
{
    public async Task<IResult> Register(UserRegisterRequest request)
    {
        var validation = await userRegisterValidator.ValidateAsync(request);
        
        if (!validation.IsValid)
            return Results.ValidationProblem(validation.ToDictionary());

        var user = await userRepository.GetByEmail(request.Email);

        if (user is not null) 
            return Results.Problem("User is already registered.");
        
        var newUser = User.Create(request.Email, request.FirstName, request.LastName, request.Password);

        await userRepository.Save(newUser);

        return Results.Ok();
    }
    
    public async Task<IResult> Login(UserLoginRequest request)
    {
        var user = await userRepository.GetByEmail(request.Email);
        
        if (user is null) return Results.Problem("User is not registered.");
        
        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.Password, request.Password) == PasswordVerificationResult.Failed)
            return Results.Problem("Password is incorrect.");
            

        return Results.Ok(new UserLoginResponse { AccessToken = jwtProvider.GenerateToken(user) });
    }
}