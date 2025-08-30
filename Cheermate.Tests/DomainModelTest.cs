using System.Collections.Generic;
using Xunit;
using Cheermate.Domain.Entities;
using Cheermate.Domain.Enums;
// Alias (optional, remove if you prefer direct TodoTask)
using DomainTask = Cheermate.Domain.Entities.TodoTask;

namespace Cheermate.Tests
{
    public class DomainModelTest
    {
        [Fact]
        public void CompletionPercentage_Is_Zero_When_No_SubTasks()
        {
            var todo = new DomainTask
            {
                Title = "Parent",
                UserId = 1,
                Priority = TaskPriority.Medium
            };

            Assert.Equal(0.0, todo.CompletionPercentage);
        }

        // Line ~33 previously failing likely referenced Entities.Task
        [Fact]
        public void CompletionPercentage_Computes_From_SubTasks()
        {
            var todo = new DomainTask
            {
                Title = "Parent",
                UserId = 1,
                Priority = TaskPriority.Medium,
                SubTasks = new List<SubTask>
                {
                    new SubTask { Title = "A", IsCompleted = true,  TaskId = 0 },
                    new SubTask { Title = "B", IsCompleted = false, TaskId = 0 },
                    new SubTask { Title = "C", IsCompleted = false, TaskId = 0 }
                }
            };

            Assert.InRange(todo.CompletionPercentage, 33.3, 33.4);
        }

        // Line ~51 also referenced the old name
        [Fact]
        public void IsOverdue_Returns_False_When_No_DueDate()
        {
            var todo = new DomainTask
            {
                Title = "No Due Date",
                UserId = 1,
                Priority = TaskPriority.Low
            };

            Assert.False(todo.IsOverdue);
        }
    }
}