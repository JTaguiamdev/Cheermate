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

    // LoggerMessage delegates for better performance
    private static readonly Action<ILogger, Exception?> LogApplyingMigrations = 
        LoggerMessage.Define(LogLevel.Information, new EventId(1), "Applying pending EF Core migrations (if any)...");
    
    private static readonly Action<ILogger, Exception?> LogDatabaseUpToDate = 
        LoggerMessage.Define(LogLevel.Information, new EventId(2), "Database is up to date.");
    
    private static readonly Action<ILogger, Exception?> LogMigrationError = 
        LoggerMessage.Define(LogLevel.Error, new EventId(3), "Error applying migrations.");

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
            LogApplyingMigrations(_logger, null);
            await db.Database.MigrateAsync(cancellationToken);
            LogDatabaseUpToDate(_logger, null);
        }
        catch (Exception ex)
        {
            LogMigrationError(_logger, ex);
            throw;
        }
    }

    public Task StopAsync(CancellationToken cancellationToken) => Task.CompletedTask;
}