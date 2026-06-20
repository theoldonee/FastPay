using FastPay.Server.Models;
using Microsoft.EntityFrameworkCore;

namespace FastPay.Server.Data;

public class FastPayDbContext(DbContextOptions<FastPayDbContext> options)
    : DbContext(options)
{
    public DbSet<Employee> Employees => Set<Employee>();

    public DbSet<PayrollCycle> PayrollCycles => Set<PayrollCycle>();

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

        modelBuilder.Entity<PayrollCycle>(entity =>
        {
            entity.ToTable("payroll_cycles", table =>
            {
                table.HasCheckConstraint(
                    "ck_payroll_cycles_date_range",
                    "end_date = start_date + 13");
                table.HasCheckConstraint(
                    "ck_payroll_cycles_status",
                    "status IN ('open', 'finalized')");
            });

            entity.HasKey(cycle => cycle.Id);

            entity.HasIndex(cycle => new { cycle.StartDate, cycle.EndDate })
                .IsUnique();

            entity.Property(cycle => cycle.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("gen_random_uuid()");

            entity.Property(cycle => cycle.StartDate)
                .HasColumnName("start_date")
                .HasColumnType("date")
                .IsRequired();

            entity.Property(cycle => cycle.EndDate)
                .HasColumnName("end_date")
                .HasColumnType("date")
                .IsRequired();

            entity.Property(cycle => cycle.Status)
                .HasColumnName("status")
                .HasDefaultValue(PayrollCycleStatuses.Open)
                .IsRequired();

            entity.Property(cycle => cycle.FinalizedAt)
                .HasColumnName("finalized_at");

            entity.Property(cycle => cycle.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql("now()")
                .IsRequired();

            entity.Property(cycle => cycle.UpdatedAt)
                .HasColumnName("updated_at")
                .HasDefaultValueSql("now()")
                .IsRequired();
        });
    }
}
