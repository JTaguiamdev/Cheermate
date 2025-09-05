using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using FlaUI.Core.AutomationElements;
using FlaUI.UIA3;
using Xunit;
// Alias to avoid collision with your Cheermate.Application namespace
using FlaUIApplication = FlaUI.Core.Application;

namespace Cheermate.UI.Tests;

public class SmokeTests : IDisposable
{
    private readonly FlaUIApplication _app;
    private readonly UIA3Automation _automation;

    public SmokeTests()
    {
        // Create seeded database before launching the app
        SetupSeededDatabase().GetAwaiter().GetResult();

        var exePath = Path.Combine(AppContext.BaseDirectory, "Cheermate.Presentation.exe");
        Assert.True(File.Exists(exePath), $"Cheermate.Presentation.exe not found at {exePath}");
        _app = FlaUIApplication.Launch(exePath);
        _automation = new UIA3Automation();
    }

    private static async Task SetupSeededDatabase()
    {
        var testDataDir = Path.Combine(AppContext.BaseDirectory, "TestData");
        var seededDbPath = Path.Combine(testDataDir, "seeded.db");
        
        // Create the seeded database
        await DatabaseSeeder.CreateSeededDatabase(seededDbPath);
        
        // Copy it to where the app expects to find the database
        var appDbPath = Path.Combine(AppContext.BaseDirectory, "cheermate.db");
        if (File.Exists(appDbPath))
        {
            File.Delete(appDbPath);
        }
        File.Copy(seededDbPath, appDbPath);
    }

    [Fact]
    public void MainForm_launches_and_has_at_least_one_row()
    {
        var mainWindow = _app.GetMainWindow(_automation, TimeSpan.FromSeconds(5));
        Assert.NotNull(mainWindow);
        Assert.Equal("Cheermate Tasks", mainWindow.Title);

        // Poll for DataGrid (DataGridView)
        AutomationElement? gridElement = null;
        var deadline = DateTime.UtcNow.AddSeconds(3);
        while (DateTime.UtcNow < deadline && gridElement == null)
        {
            gridElement = mainWindow.FindFirstDescendant(
                cf => cf.ByControlType(FlaUI.Core.Definitions.ControlType.DataGrid));
            if (gridElement == null)
                Thread.Sleep(100);
        }

        Assert.NotNull(gridElement);

        Thread.Sleep(200); // allow for data binding
        var grid = gridElement!.AsGrid();
        Assert.NotNull(grid);
        Assert.True(grid.Rows.Length >= 1, "Expected at least one seeded task row.");
    }

    public void Dispose()
    {
        try { if (!_app.HasExited) _app.Close(); } catch { }
        _automation.Dispose();
    }
}