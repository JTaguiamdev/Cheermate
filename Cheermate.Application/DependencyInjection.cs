using Microsoft.Extensions.DependencyInjection;

namespace Cheermate.Application;

public static class DependencyInjection
{
    // Add Application-layer services (mediators, validators, etc.) here later.
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Currently nothing specific. Keep placeholder method for growth.
        return services;
    }
}