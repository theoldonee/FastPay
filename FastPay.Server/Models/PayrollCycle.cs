namespace FastPay.Server.Models;

public class PayrollCycle
{
    public Guid Id { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly EndDate { get; set; }

    public string Status { get; set; } = PayrollCycleStatuses.Open;

    public DateTimeOffset? FinalizedAt { get; set; }

    public DateTimeOffset CreatedAt { get; set; }

    public DateTimeOffset UpdatedAt { get; set; }
}
