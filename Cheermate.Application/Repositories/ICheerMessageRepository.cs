using Cheermate.Domain.Entities;

namespace Cheermate.Application.Repositories;

public interface ICheerMessageRepository
{
    Task<List<CheerMessage>> GetRecentAsync(int take = 50, CancellationToken ct = default);
    Task<CheerMessage> AddAsync(string text, CancellationToken ct = default);
}