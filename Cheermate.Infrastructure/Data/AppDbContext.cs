using Cheermate.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Cheermate.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<CheerMessage> CheerMessages => Set<CheerMessage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<CheerMessage>(e =>
        {
            e.ToTable("CheerMessages");
            e.HasKey(x => x.Id);
            e.Property(x => x.Text).IsRequired().HasMaxLength(500);
            e.Property(x => x.CreatedUtc).IsRequired();
        });
    }
}