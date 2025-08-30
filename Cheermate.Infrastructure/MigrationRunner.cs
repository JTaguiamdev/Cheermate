using Cheermate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Cheermate.Infrastructure;

public class MigrationRunner : IHostedService
{
    private readonly IServiceProvider _provider;
    private readonly ILogger<MigrationRunner> _logger;

    public MigrationRunner(IServiceProvider provider, ILogger<MigrationRunner> logger)
    {
        _provider = provider;
        _logger = logger;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        try
        {
            using var scope = _provider.CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            _logger.LogInformation("Applying pending EF Core migrations (if any)...");
            await db.Database.MigrateAsync(cancellationToken);
            _logger.LogInformation("Database is up to date.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error applying migrations.");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}