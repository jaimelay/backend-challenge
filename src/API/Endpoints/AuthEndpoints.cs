using Application.Dtos.Requests;
using Application.Dtos.Responses;
using Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace desafioBackend.Endpoints;

public static class AuthEndpoints
{
    public static IEndpointRouteBuilder UseAuthEndpoints(this IEndpointRouteBuilder endpoints)
    {
        var authEndpoints = endpoints.MapGroup("/auth");

        authEndpoints.MapPost("/register", async (IAuthAppService authAppService, [FromBody] UserRegisterRequest request) 
                => await authAppService.Register(request))
            .Produces<IResult>();
        
        authEndpoints.MapPost("/login", async (IAuthAppService authAppService, [FromBody] UserLoginRequest request) 
            => await authAppService.Login(request))
            .Produces<IResult>();

        return authEndpoints;
    }
}