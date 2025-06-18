using Application.Dtos.Requests;
using Application.Interfaces;
using Application.Services;
using Application.Validators;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class ServiceCollectionsExtensionsApplication
{
    public static IServiceCollection AddApplicationDependency(this IServiceCollection services)
    {
        ArgumentNullException.ThrowIfNull(services);

        services.AddScoped<IAuthAppService, AuthAppService>();
        services.AddScoped<ITransferAppService, TransferAppService>();
        services.AddScoped<IWalletAppService, WalletAppService>();

        services.AddScoped<IValidator<UserRegisterRequest>, UserRegisterValidator>();
        
        return services;
    }
}