using Cheermate.Application.Repositories;
using Cheermate.Domain.Entities;
using Cheermate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cheermate.Infrastructure.Repositories
{
    public class TodoTaskRepository : ITodoTaskRepository
    {
        private readonly CheermateDbContext _db;
        public TodoTaskRepository(CheermateDbContext db) => _db = db;

        public Task<List<TodoTask>> GetRecentAsync(int take = 50, CancellationToken ct = default)
            => _db.TodoTasks
                  .OrderByDescending(t => t.CreatedAt)
                  .Take(take)
                  .AsNoTracking()
                  .ToListAsync(ct);
    }
}
