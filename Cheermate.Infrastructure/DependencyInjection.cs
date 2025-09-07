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

        // Existing (CheerMessages) context
        services.AddDbContext<AppDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });

        // Tasks / Users / SubTasks context
        services.AddDbContext<CheermateDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });

        services.AddScoped<ICheerMessageRepository, CheerMessageRepository>();
        services.AddScoped<ITodoTaskRepository, TodoTaskRepository>();
        services.AddSingleton<IGreetingService, SystemGreetingService>();

        if (applyMigrations)
        {
            services.AddHostedService<MigrationRunner>();
        }

        return services;
    }
}
