using Microsoft.EntityFrameworkCore;
using Cheermate.Infrastructure.Data;
using System.IO;

namespace Cheermate.UI.Tests;

public static class DatabaseSeeder
{
    public static async Task CreateSeededDatabase(string filePath)
    {
        // Ensure directory exists
        var directory = Path.GetDirectoryName(filePath);
        if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        // Delete existing file if it exists
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }

        // Create database with seeded data
        var options = new DbContextOptionsBuilder<CheermateDbContext>()
            .UseSqlite($"Data Source={filePath}")
            .Options;

        using var context = new CheermateDbContext(options);
        await context.Database.EnsureCreatedAsync();
        
        // The seeded data from the CheermateDbContext.Seed method will be automatically applied
        // when EnsureCreated is called since it's part of OnModelCreating
    }
}