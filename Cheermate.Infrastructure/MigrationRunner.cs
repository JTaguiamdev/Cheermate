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

            async Task MigrateIfExists<TContext>(IServiceScope s, string name) where TContext : DbContext
            {
                var ctx = s.ServiceProvider.GetService<TContext>();
                if (ctx != null)
                {
                    _logger.LogInformation("Applying migrations for {Context}...", name);
                    await ctx.Database.MigrateAsync(cancellationToken);
                    _logger.LogInformation("{Context} up to date.", name);
                }
            }

            await MigrateIfExists<AppDbContext>(scope, nameof(AppDbContext));
            await MigrateIfExists<CheermateDbContext>(scope, nameof(CheermateDbContext));
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error applying migrations.");
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}
