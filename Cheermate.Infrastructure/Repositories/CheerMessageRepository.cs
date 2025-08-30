using Cheermate.Application.Repositories;
using Cheermate.Domain.Entities;
using Cheermate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cheermate.Infrastructure.Repositories;

public class CheerMessageRepository : ICheerMessageRepository
{
    private readonly AppDbContext _db;

    public CheerMessageRepository(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<CheerMessage>> GetRecentAsync(int take = 50, CancellationToken ct = default)
    {
        return await _db.CheerMessages
            .OrderByDescending(c => c.CreatedUtc)
            .Take(take)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<CheerMessage> AddAsync(string text, CancellationToken ct = default)
    {
        var entity = new CheerMessage
        {
            Text = text.Trim(),
            CreatedUtc = DateTime.UtcNow
        };
        _db.CheerMessages.Add(entity);
        await _db.SaveChangesAsync(ct);
        return entity;
    }
}