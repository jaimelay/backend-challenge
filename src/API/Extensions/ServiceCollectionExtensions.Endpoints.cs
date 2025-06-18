using desafioBackend.Endpoints;

namespace desafioBackend.Extensions;

public static class ServiceCollectionExtensionsEndpoints
{
    public static IEndpointRouteBuilder UseEndpoints(this IEndpointRouteBuilder endpoints)
    {
        return endpoints.MapGroup("/api")
            .UseAuthEndpoints()
            .UseTransferEndpoints()
            .UseWalletEndpoints();
    }
}