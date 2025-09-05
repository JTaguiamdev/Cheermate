using Cheermate.Application.Repositories;
using Cheermate.Domain.Entities;
using Cheermate.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Cheermate.Infrastructure.Repositories;

public class TodoTaskRepository : ITodoTaskRepository
{
    private readonly CheermateDbContext _db;

    public TodoTaskRepository(CheermateDbContext db)
    {
        _db = db;
    }

    public async Task<List<TodoTask>> GetAllAsync(CancellationToken ct = default)
    {
        return await _db.TodoTasks
            .Include(t => t.SubTasks)
            .Include(t => t.User)
            .OrderBy(t => t.CreatedAt)
            .AsNoTracking()
            .ToListAsync(ct);
    }

    public async Task<TodoTask?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        return await _db.TodoTasks
            .Include(t => t.SubTasks)
            .Include(t => t.User)
            .FirstOrDefaultAsync(t => t.Id == id, ct);
    }

    public async Task<TodoTask> UpdateAsync(TodoTask task, CancellationToken ct = default)
    {
        _db.TodoTasks.Update(task);
        await _db.SaveChangesAsync(ct);
        return task;
    }

    public async Task SaveChangesAsync(CancellationToken ct = default)
    {
        await _db.SaveChangesAsync(ct);
    }
}