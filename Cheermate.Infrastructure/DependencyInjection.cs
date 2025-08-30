using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Cheermate.Application.Services;
using Cheermate.Infrastructure.Services;
using Cheermate.Infrastructure.Data;
using Cheermate.Application.Repositories;
using Cheermate.Infrastructure.Repositories;

namespace Cheermate.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructureServices(
        this IServiceCollection services,
        string? connectionString,
        bool applyMigrations = true)
    {
        if (string.IsNullOrWhiteSpace(connectionString))
            throw new ArgumentException("Connection string is null or empty.", nameof(connectionString));

        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });

        // Repositories
        services.AddScoped<ICheerMessageRepository, CheerMessageRepository>();

        // Existing greeting service
        services.AddSingleton<IGreetingService, SystemGreetingService>();

        // Optional: ensure DB created / migrations applied at runtime (only for dev; remove in prod)
        if (applyMigrations)
        {
            services.AddHostedService<MigrationRunner>();
        }

        return services;
    }
}