using EmployeeLeaveAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace EmployeeLeaveAPI.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<Leave> Leaves { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.Property(e => e.EmployeeNumber).IsRequired();
                entity.HasIndex(e => e.EmployeeNumber).IsUnique();
                entity.Property(e => e.Name).IsRequired().HasMaxLength(200);
                entity.Property(e => e.Qualification).HasMaxLength(200);
            });

            modelBuilder.Entity<Leave>(entity =>
            {
                entity.HasKey(l => l.Id);
                entity.Property(l => l.LeaveType).IsRequired();
                entity.Property(l => l.StartDate).IsRequired();
                entity.Property(l => l.DurationDays).IsRequired();
                entity.Ignore(l => l.EndDate);

                entity.HasOne(l => l.Employee)
                      .WithMany(e => e.Leaves)
                      .HasForeignKey(l => l.EmployeeId)
                      .OnDelete(DeleteBehavior.Cascade);
            });
        }
    }
}
