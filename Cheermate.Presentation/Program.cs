using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Cheermate.Application;
using Cheermate.Infrastructure;
using Cheermate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cheermate.Presentation;

internal static class Program
{
    [STAThread]
    static async Task Main()
    {
        ApplicationConfiguration.Initialize();

        // Configuration
        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // DI container
        var services = new ServiceCollection();

        services.AddSingleton<IConfiguration>(config);

        services
            .AddLogging(b =>
            {
                b.AddConsole();
                b.AddDebug();
                b.SetMinimumLevel(LogLevel.Information);
            })
            .AddApplicationServices();

        var connectionString = config.GetConnectionString("Default");
        services.AddInfrastructureServices(connectionString);

        services.AddTransient<MainForm>();

        using var provider = services.BuildServiceProvider();

        // Apply migrations
        using (var scope = provider.CreateScope())
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.MigrateAsync();
        }

        var mainForm = provider.GetRequiredService<MainForm>();

        // Fully qualified to avoid ambiguity
        System.Windows.Forms.Application.Run(mainForm);
    }
}