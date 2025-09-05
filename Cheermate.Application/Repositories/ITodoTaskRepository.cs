using Cheermate.Domain.Entities;

namespace Cheermate.Application.Repositories;

public interface ITodoTaskRepository
{
    Task<List<TodoTask>> GetAllAsync(CancellationToken ct = default);
    Task<TodoTask?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<TodoTask> UpdateAsync(TodoTask task, CancellationToken ct = default);
    Task SaveChangesAsync(CancellationToken ct = default);
}