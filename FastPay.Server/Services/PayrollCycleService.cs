using FastPay.Server.Data;
using FastPay.Server.Errors;
using FastPay.Server.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace FastPay.Server.Services;

public sealed class PayrollCycleService(FastPayDbContext dbContext)
{
    public async Task<IReadOnlyList<PayrollCycle>> ListAsync(CancellationToken cancellationToken)
    {
        return await dbContext.PayrollCycles
            .AsNoTracking()
            .OrderByDescending(cycle => cycle.StartDate)
            .ToListAsync(cancellationToken);
    }

    public async Task<PayrollCycle> GetAsync(Guid cycleId, CancellationToken cancellationToken)
    {
        return await dbContext.PayrollCycles
            .AsNoTracking()
            .SingleOrDefaultAsync(cycle => cycle.Id == cycleId, cancellationToken)
            ?? throw NotFound(cycleId);
    }

    public async Task<PayrollCycle> CreateAsync(
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken)
    {
        ValidateDates(startDate, endDate);
        await EnsureNoOverlapAsync(startDate, endDate, null, cancellationToken);

        var now = DateTimeOffset.UtcNow;
        var cycle = new PayrollCycle
        {
            Id = Guid.NewGuid(),
            StartDate = startDate,
            EndDate = endDate,
            Status = PayrollCycleStatuses.Open,
            CreatedAt = now,
            UpdatedAt = now
        };

        dbContext.PayrollCycles.Add(cycle);
        await dbContext.SaveChangesAsync(cancellationToken);

        return cycle;
    }

    public async Task<PayrollCycle> UpdateAsync(
        Guid cycleId,
        DateOnly startDate,
        DateOnly endDate,
        CancellationToken cancellationToken)
    {
        ValidateDates(startDate, endDate);

        var cycle = await dbContext.PayrollCycles
            .SingleOrDefaultAsync(cycle => cycle.Id == cycleId, cancellationToken)
            ?? throw NotFound(cycleId);

        if (cycle.Status != PayrollCycleStatuses.Open)
        {
            throw new ApiException(
                StatusCodes.Status409Conflict,
                "Payroll cycle cannot be updated",
                "Only open payroll cycles can be updated.");
        }

        await EnsureNoOverlapAsync(startDate, endDate, cycleId, cancellationToken);

        cycle.StartDate = startDate;
        cycle.EndDate = endDate;
        cycle.UpdatedAt = DateTimeOffset.UtcNow;

        await dbContext.SaveChangesAsync(cancellationToken);

        return cycle;
    }

    private static void ValidateDates(DateOnly startDate, DateOnly endDate)
    {
        if (endDate.DayNumber - startDate.DayNumber != 13)
        {
            throw new ApiException(
                StatusCodes.Status400BadRequest,
                "Invalid payroll cycle dates",
                "A payroll cycle must contain exactly 14 calendar days inclusive.");
        }
    }

    private async Task EnsureNoOverlapAsync(
        DateOnly startDate,
        DateOnly endDate,
        Guid? excludedCycleId,
        CancellationToken cancellationToken)
    {
        var overlaps = await dbContext.PayrollCycles.AnyAsync(
            cycle =>
                (!excludedCycleId.HasValue || cycle.Id != excludedCycleId.Value)
                && cycle.StartDate <= endDate
                && cycle.EndDate >= startDate,
            cancellationToken);

        if (overlaps)
        {
            throw new ApiException(
                StatusCodes.Status409Conflict,
                "Payroll cycle overlaps an existing cycle",
                "Payroll cycles cannot overlap.");
        }
    }

    private static ApiException NotFound(Guid cycleId) =>
        new(
            StatusCodes.Status404NotFound,
            "Payroll cycle not found",
            $"Payroll cycle '{cycleId}' was not found.");
}
