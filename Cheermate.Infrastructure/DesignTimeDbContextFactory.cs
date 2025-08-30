using Cheermate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Cheermate.Infrastructure;

// Enables 'dotnet ef migrations add' without needing the running app.
public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        // Fallback connection for design-time; could read from env variable.
        var builder = new DbContextOptionsBuilder<AppDbContext>();
        builder.UseSqlite("Data Source=cheermate.db");
        return new AppDbContext(builder.Options);
    }
}