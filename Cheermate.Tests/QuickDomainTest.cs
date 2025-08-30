using Xunit;
using Cheermate.Domain.Entities;
using Cheermate.Domain.Enums;
using System.Collections.Generic;

namespace Cheermate.Tests
{
    public class QuickDomainTest
    {
        [Fact]
        public void CompletionPercentage_Computes_From_SubTasks()
        {
            var task = new TodoTask
            {
                Title = "Parent",
                Priority = TaskPriority.Medium,
                UserId = 1,
                SubTasks = new List<SubTask>
                {
                    new SubTask { Title = "A", IsCompleted = true,  TaskId = 0 }, // TaskId will be ignored in pure in‑memory test
                    new SubTask { Title = "B", IsCompleted = false, TaskId = 0 },
                    new SubTask { Title = "C", IsCompleted = false, TaskId = 0 }
                }
            };

            // 1 of 3 complete => 33.(3) which your code will treat as 33.333...
            Assert.InRange(task.CompletionPercentage, 33.3, 33.4);
        }
    }
}