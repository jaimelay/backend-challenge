using Application.Dtos.Requests;
using Application.Dtos.Responses;
using Microsoft.AspNetCore.Http;

namespace Application.Interfaces;

public interface IAuthAppService
{
    Task<IResult> Register(UserRegisterRequest request);
    Task<IResult> Login(UserLoginRequest request);
}