using System.ComponentModel.DataAnnotations;
using Cheermate.Domain.Enums;

namespace Cheermate.Domain.Entities
{
    public class User : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(100)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(255)]
        public string PasswordHash { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string LastName { get; set; } = string.Empty;

        [Required]
        public DateTime DateOfBirth { get; set; }

        // Calculated property for age
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - DateOfBirth.Year;
                if (DateOfBirth.Date > today.AddYears(-age)) age--;
                return age;
            }
        }

        [StringLength(200)]
        public string SecurityQuestion { get; set; } = string.Empty;

        [StringLength(100)]
        public string SecurityAnswer { get; set; } = string.Empty;

        [Required]
        public ReminderPersonality SelectedPersonality { get; set; }

        // Navigation property
        public virtual ICollection<TodoTask> Tasks { get; set; } = new List<TodoTask>();

        public string FullName => $"{FirstName} {LastName}";
    }
}