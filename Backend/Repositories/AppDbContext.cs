using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System.IO;

namespace Repositories
{

	public class AppDbContext : DbContext
	{
		public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

		public DbSet<User> Users { get; set; }
		public DbSet<Habit> Habits { get; set; }
		public DbSet<HabitProgress> HabitProgresses { get; set; }
		public DbSet<HabitLog> HabitLogs { get; set; }
		public DbSet<Prize> Prizes { get; set; }
		public DbSet<UserPrize> UserPrizes { get; set; }
		public DbSet<Notification> Notifications { get; set; }
		public DbSet<Category> Categories { get; set; }

		protected override void OnModelCreating(ModelBuilder modelBuilder)
		{
			base.OnModelCreating(modelBuilder);

			modelBuilder.Entity<UserPrize>().HasKey(up => up.Id);

			modelBuilder.Entity<UserPrize>()
				.HasOne(up => up.User)
				.WithMany(u => u.UserPrizes)
				.HasForeignKey(up => up.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<UserPrize>()
				.HasOne(up => up.Prize)
				.WithMany(p => p.UserPrizes)
				.HasForeignKey(up => up.PrizeId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Habit>()
				.HasOne(h => h.User)
				.WithMany(u => u.Habits)
				.HasForeignKey(h => h.UserId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Habit>()
				.HasMany(h => h.HabitProgresses)
				.WithOne(p => p.Habit)
				.HasForeignKey(p => p.HabitId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Habit>()
				.HasOne(h => h.Category)
				.WithMany(c => c.Habits)
				.HasForeignKey(h => h.CategoryId)
				.OnDelete(DeleteBehavior.Restrict);

			modelBuilder.Entity<HabitProgress>()
				.HasOne(hl => hl.Habit)
				.WithMany(h => h.HabitProgresses)
				.HasForeignKey(hl => hl.HabitId)
				.OnDelete(DeleteBehavior.Cascade);

			modelBuilder.Entity<Notification>()
				.HasOne(n => n.User)
				.WithMany(u => u.Notifications)
				.HasForeignKey(n => n.UserId)
				.OnDelete(DeleteBehavior.Cascade);
		}
	}
}
