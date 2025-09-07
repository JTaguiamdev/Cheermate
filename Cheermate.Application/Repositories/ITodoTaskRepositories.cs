using Cheermate.Domain.Entities;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Cheermate.Application.Repositories
{
    public interface ITodoTaskRepository
    {
        Task<List<TodoTask>> GetRecentAsync(int take = 50, CancellationToken ct = default);
    }
}
