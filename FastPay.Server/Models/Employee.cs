namespace FastPay.Server.Models;

public class Employee
{
    public Guid Id { get; set; }

    public string FullName { get; set; } = string.Empty;

    public decimal HourlyRate { get; set; }

    public bool IsActive { get; set; } = true;

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}