using CrossCutting.Auth;
using CrossCutting.Auth.Interfaces;
using CrossCutting.Interfaces.Repositories;
using CrossCutting.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CrossCutting;

public static class ServiceCollectionsExtensionsInfra
{
    public static IServiceCollection AddInfraDependency(
        this IServiceCollection services, 
        IConfiguration configuration,
        IWebHostEnvironment environment)
    {
        ArgumentNullException.ThrowIfNull(services);
        ArgumentNullException.ThrowIfNull(configuration);

        services.AddScoped<IJwtProvider, JwtProvider>();
        
        services.AddScoped<ITransferRepository, TransferRepository>();
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IWalletRepository, WalletRepository>();
        
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("Db")).NpgSqlEnableLegacyTimestampBehavior());
        
        return services;
    }

    public static DbContextOptionsBuilder NpgSqlEnableLegacyTimestampBehavior(
        this DbContextOptionsBuilder options,
        bool isEnabled = true)
    {
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", isEnabled);
        return options;
    }
    
    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        
        using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        dbContext.Database.Migrate();
    }
}