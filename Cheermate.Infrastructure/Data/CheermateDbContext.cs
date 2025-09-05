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

            Seed(modelBuilder);
        }

        private static void Seed(ModelBuilder modelBuilder)
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
                    CreatedAt = baseDate,
                    UpdatedAt = baseDate
                }
            );

            modelBuilder.Entity<SubTask>().HasData(
                new SubTask { Id = 1, Title = "Set up Domain Models", IsCompleted = true, TaskId = 1, CreatedAt = baseDate, UpdatedAt = baseDate }
            );
        }
    }
}