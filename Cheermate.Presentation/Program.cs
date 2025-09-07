using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Cheermate.Application;          // extension methods (AddApplicationServices)
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

        var config = new ConfigurationBuilder()
            .SetBasePath(AppContext.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

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
        services.AddInfrastructureServices(connectionString, applyMigrations: false); // we migrate explicitly below

        services.AddScoped<MainForm>();

        using var provider = services.BuildServiceProvider();

        // Explicit migrations (run both contexts if registered)
        using (var scope = provider.CreateScope())
        {
            var appDb = scope.ServiceProvider.GetService<AppDbContext>();
            if (appDb != null)
                await appDb.Database.MigrateAsync();

            var tasksDb = scope.ServiceProvider.GetService<CheermateDbContext>();
            if (tasksDb != null)
                await tasksDb.Database.MigrateAsync();
        }

        var mainForm = provider.GetRequiredService<MainForm>();

        // Fully qualify WinForms Application to avoid collision with Cheermate.Application namespace
        System.Windows.Forms.Application.Run(mainForm);
    }
}
