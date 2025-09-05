using Microsoft.EntityFrameworkCore;
using Cheermate.Domain.Entities;

namespace Cheermate.Infrastructure.Data
{
    public class CheermateDbContext : DbContext
    {
        public CheermateDbContext(DbContextOptions<CheermateDbContext> options) : base(options) { }

        public DbSet<User> Users => Set<User>();
        public DbSet<TodoTask> TodoTasks => Set<TodoTask>();
        public DbSet<SubTask> SubTasks => Set<SubTask>();
        public DbSet<CheerMessage> CheerMessages => Set<CheerMessage>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<TodoTask>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Title).IsRequired().HasMaxLength(200);
                e.Property(x => x.Description).HasMaxLength(1000);
                e.Property(x => x.Priority).HasConversion<string>();
                e.HasOne(x => x.User)
                 .WithMany()
                 .HasForeignKey(x => x.UserId)
                 .OnDelete(DeleteBehavior.Cascade);

                e.HasMany(x => x.SubTasks)
                 .WithOne(st => st.Task)
                 .HasForeignKey(st => st.TaskId)
                 .OnDelete(DeleteBehavior.Cascade);
            });

            modelBuilder.Entity<SubTask>(e =>
            {
                e.HasKey(x => x.Id);
                e.Property(x => x.Title).IsRequired().HasMaxLength(100);
            });

            modelBuilder.Entity<CheerMessage>(e =>
            {
                e.ToTable("CheerMessages");
                e.HasKey(x => x.Id);
                e.Property(x => x.Text).IsRequired().HasMaxLength(500);
                e.Property(x => x.CreatedUtc).IsRequired();
            });

            Seed(modelBuilder);
        }

        private void Seed(ModelBuilder modelBuilder)
        {
            var baseDate = DateTime.UtcNow;

            modelBuilder.Entity<User>().HasData(new User
            {
                Id = 1,
                Username = "JTaguiamdev",
                Email = "jtaguiam@cheermate.com",
                FirstName = "JT",
                LastName = "Aguiam",
                DateOfBirth = new DateTime(1995, 5, 15),
                PasswordHash = "seed_hash",
                SelectedPersonality = Domain.Enums.ReminderPersonality.GenZConyo,
                CreatedAt = baseDate,
                UpdatedAt = baseDate
            });

            modelBuilder.Entity<TodoTask>().HasData(
                new TodoTask
                {
                    Id = 1,
                    Title = "Complete Cheermate App",
                    Description = "Finish building the task reminder system",
                    Priority = Domain.Enums.TaskPriority.High,
                    UserId = 1,
                    IsCompleted = false,
                    DueDate = DateTime.UtcNow.AddDays(7),
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                },
                new TodoTask
                {
                    Id = 2,
                    Title = "Review Code Quality",
                    Description = "Review and refactor existing codebase",
                    Priority = Domain.Enums.TaskPriority.Medium,
                    UserId = 1,
                    IsCompleted = true,
                    DueDate = DateTime.UtcNow.AddDays(-2),
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                },
                new TodoTask
                {
                    Id = 3,
                    Title = "Update Documentation",
                    Description = "Update user documentation and API docs",
                    Priority = Domain.Enums.TaskPriority.Low,
                    UserId = 1,
                    IsCompleted = false,
                    DueDate = DateTime.UtcNow.AddDays(-1), // Overdue
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                },
                new TodoTask
                {
                    Id = 4,
                    Title = "Prepare Demo",
                    Description = "Prepare demo for stakeholder presentation",
                    Priority = Domain.Enums.TaskPriority.High,
                    UserId = 1,
                    IsCompleted = false,
                    DueDate = DateTime.UtcNow.AddDays(2), // Upcoming
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                }
            );

            modelBuilder.Entity<SubTask>().HasData(
                new SubTask { Id = 1, Title = "Set up Domain Models", IsCompleted = true, TaskId = 1, CreatedAt = baseDate, UpdatedAt = baseDate },
                new SubTask { Id = 2, Title = "Create UI Components", IsCompleted = false, TaskId = 1, CreatedAt = baseDate, UpdatedAt = baseDate },
                new SubTask { Id = 3, Title = "Write Unit Tests", IsCompleted = true, TaskId = 2, CreatedAt = baseDate, UpdatedAt = baseDate },
                new SubTask { Id = 4, Title = "Review Pull Requests", IsCompleted = true, TaskId = 2, CreatedAt = baseDate, UpdatedAt = baseDate }
            );
        }
    }
}