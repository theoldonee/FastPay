using FastPay.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace FastPay.Server.Data;

public class FastPayDbContext(DbContextOptions<FastPayDbContext> options)
    : DbContext(options)
{
    public DbSet<Employee> Employees => Set<Employee>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Employee>(entity =>
        {
            entity.ToTable("employees", table =>
            {
                table.HasCheckConstraint(
                    "ck_employees_hourly_rate",
                    "hourly_rate > 0 AND hourly_rate <= 100000.00");
            });

            entity.HasKey(employee => employee.Id);

            entity.Property(employee => employee.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(employee => employee.FullName)
                .HasColumnName("full_name")
                .HasMaxLength(120)
                .IsRequired();

            entity.Property(employee => employee.HourlyRate)
                .HasColumnName("hourly_rate")
                .HasPrecision(12, 2)
                .IsRequired();

            entity.Property(employee => employee.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true)
                .IsRequired();

            entity.Property(employee => employee.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("now()")
                .IsRequired();

            entity.Property(employee => employee.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("now()")
                .IsRequired();
        });
    }
}