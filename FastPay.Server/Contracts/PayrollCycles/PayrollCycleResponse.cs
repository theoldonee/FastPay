using FastPay.Server.Models;

namespace FastPay.Server.Contracts.PayrollCycles;

public sealed record PayrollCycleResponse(
    Guid Id,
    DateOnly StartDate,
    DateOnly EndDate,
    string Status,
    DateTimeOffset? FinalizedAt,
    DateTimeOffset CreatedAt,
    DateTimeOffset UpdatedAt)
{
    public static PayrollCycleResponse FromModel(PayrollCycle cycle) =>
        new(
            cycle.Id,
            cycle.StartDate,
            cycle.EndDate,
            cycle.Status,
            cycle.FinalizedAt,
            cycle.CreatedAt,
            cycle.UpdatedAt);
}
