using Microsoft.Extensions.DependencyInjection;

namespace Notadesigner.Approximato.Messaging.ServiceRegistration;

public static class ServiceCollectionServiceExtensions
{
    public static IServiceCollection AddKeyedAndDefaultScoped<TService, TImplementation>(this IServiceCollection services, string key)
        where TService : class
        where TImplementation : class, TService
    {
        services.AddKeyedScoped<TService, TImplementation>(key);
        services.AddScoped(provider => provider.GetRequiredKeyedService<TService>(key));

        return services;
    }
}