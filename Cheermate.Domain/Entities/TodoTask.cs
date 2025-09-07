using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Cheermate.Domain.Enums;

namespace Cheermate.Domain.Entities
{
    public class TodoTask : BaseEntity
    {
        [Required]
        [StringLength(200)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        public DateTime? DueDate { get; set; }

        public bool IsCompleted { get; set; }

        [Required]
        public TaskPriority Priority { get; set; } = TaskPriority.Medium;

        [Required]
        public int UserId { get; set; }

        public virtual User User { get; set; } = null!;

        public virtual ICollection<SubTask> SubTasks { get; set; } = new List<SubTask>();

        // Computed convenience properties (not mapped by EF Core automatically; add [NotMapped] if needed)
        public bool IsOverdue => !IsCompleted && DueDate.HasValue && DueDate < DateTime.UtcNow;
        public bool IsUpcoming => !IsCompleted && DueDate.HasValue && DueDate <= DateTime.UtcNow.AddDays(3);
        public int CompletedSubTasksCount => SubTasks.Count(st => st.IsCompleted);
        public int TotalSubTasksCount => SubTasks.Count;
        public double CompletionPercentage =>
            IsCompleted ? 100.0 :
            TotalSubTasksCount == 0 ? 0.0 :
            (double)CompletedSubTasksCount / TotalSubTasksCount * 100.0;
    }
}
