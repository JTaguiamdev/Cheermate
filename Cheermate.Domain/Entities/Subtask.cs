using System.ComponentModel.DataAnnotations;

namespace Cheermate.Domain.Entities
{
    public class SubTask : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        public bool IsCompleted { get; set; }

        [Required]
        public int TaskId { get; set; }   // FK column

        public virtual TodoTask Task { get; set; } = null!; // navigation
    }
}