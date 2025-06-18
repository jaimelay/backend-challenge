using CrossCutting.Auth;
using CrossCutting.Auth.Interfaces;
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
        
        services.AddDbContext<AppDbContext>(options => options.UseNpgsql(configuration.GetConnectionString("Db")));
        
        return services;
    }

    public static void ApplyMigrations(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.CreateScope();
        
        using var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        
        dbContext.Database.Migrate();
    }
}