using FastPay.Server.Data;
using FastPay.Server.Errors;
using FastPay.Server.Models;
using FastPay.Server.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace FastPay.Server.Tests.Services;

public sealed class PayrollCycleServiceTests
{
    [Fact]
    public async Task CreateAsync_AcceptsExactlyFourteenDaysInclusive()
    {
        await using var context = CreateContext();
        var service = new PayrollCycleService(context);

        var cycle = await service.CreateAsync(
            new DateOnly(2026, 6, 1),
            new DateOnly(2026, 6, 14),
            CancellationToken.None);

        Assert.Equal(PayrollCycleStatuses.Open, cycle.Status);
        Assert.Equal(13, cycle.EndDate.DayNumber - cycle.StartDate.DayNumber);
        Assert.NotEqual(Guid.Empty, cycle.Id);
    }

    [Theory]
    [InlineData(12)]
    [InlineData(14)]
    [InlineData(-1)]
    public async Task CreateAsync_RejectsInvalidDateRanges(int daysAfterStart)
    {
        await using var context = CreateContext();
        var service = new PayrollCycleService(context);
        var startDate = new DateOnly(2026, 6, 1);

        var exception = await Assert.ThrowsAsync<ApiException>(() =>
            service.CreateAsync(
                startDate,
                startDate.AddDays(daysAfterStart),
                CancellationToken.None));

        Assert.Equal(StatusCodes.Status400BadRequest, exception.StatusCode);
    }

    [Fact]
    public async Task CreateAsync_RejectsExactDuplicate()
    {
        await using var context = CreateContext();
        var service = new PayrollCycleService(context);
        var startDate = new DateOnly(2026, 6, 1);
        var endDate = new DateOnly(2026, 6, 14);
        await service.CreateAsync(startDate, endDate, CancellationToken.None);

        var exception = await Assert.ThrowsAsync<ApiException>(() =>
            service.CreateAsync(startDate, endDate, CancellationToken.None));

        Assert.Equal(StatusCodes.Status409Conflict, exception.StatusCode);
    }

    [Theory]
    [InlineData("2026-05-25", "2026-06-07")]
    [InlineData("2026-06-08", "2026-06-21")]
    [InlineData("2026-05-30", "2026-06-12")]
    public async Task CreateAsync_RejectsOverlappingCycles(string start, string end)
    {
        await using var context = CreateContext();
        var service = new PayrollCycleService(context);
        await service.CreateAsync(
            new DateOnly(2026, 6, 1),
            new DateOnly(2026, 6, 14),
            CancellationToken.None);

        var exception = await Assert.ThrowsAsync<ApiException>(() =>
            service.CreateAsync(
                DateOnly.Parse(start),
                DateOnly.Parse(end),
                CancellationToken.None));

        Assert.Equal(StatusCodes.Status409Conflict, exception.StatusCode);
    }

    [Fact]
    public async Task CreateAsync_AllowsAdjacentCycles()
    {
        await using var context = CreateContext();
        var service = new PayrollCycleService(context);
        await service.CreateAsync(
            new DateOnly(2026, 6, 1),
            new DateOnly(2026, 6, 14),
            CancellationToken.None);

        var adjacent = await service.CreateAsync(
            new DateOnly(2026, 6, 15),
            new DateOnly(2026, 6, 28),
            CancellationToken.None);

        Assert.Equal(new DateOnly(2026, 6, 15), adjacent.StartDate);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesAnOpenCycleAndExcludesItselfFromOverlapCheck()
    {
        await using var context = CreateContext();
        var service = new PayrollCycleService(context);
        var cycle = await service.CreateAsync(
            new DateOnly(2026, 6, 1),
            new DateOnly(2026, 6, 14),
            CancellationToken.None);

        var updated = await service.UpdateAsync(
            cycle.Id,
            new DateOnly(2026, 6, 1),
            new DateOnly(2026, 6, 14),
            CancellationToken.None);

        Assert.Equal(cycle.Id, updated.Id);
        Assert.Equal(PayrollCycleStatuses.Open, updated.Status);
    }

    [Fact]
    public async Task UpdateAsync_RejectsAnOverlapWithAnotherCycle()
    {
        await using var context = CreateContext();
        var service = new PayrollCycleService(context);
        var first = await service.CreateAsync(
            new DateOnly(2026, 6, 1),
            new DateOnly(2026, 6, 14),
            CancellationToken.None);
        await service.CreateAsync(
            new DateOnly(2026, 6, 29),
            new DateOnly(2026, 7, 12),
            CancellationToken.None);

        var exception = await Assert.ThrowsAsync<ApiException>(() =>
            service.UpdateAsync(
                first.Id,
                new DateOnly(2026, 6, 20),
                new DateOnly(2026, 7, 3),
                CancellationToken.None));

        Assert.Equal(StatusCodes.Status409Conflict, exception.StatusCode);
    }

    [Fact]
    public async Task UpdateAsync_RejectsFinalizedCycle()
    {
        await using var context = CreateContext();
        var cycle = NewCycle(
            new DateOnly(2026, 6, 1),
            new DateOnly(2026, 6, 14),
            PayrollCycleStatuses.Finalized);
        context.PayrollCycles.Add(cycle);
        await context.SaveChangesAsync();
        var service = new PayrollCycleService(context);

        var exception = await Assert.ThrowsAsync<ApiException>(() =>
            service.UpdateAsync(
                cycle.Id,
                cycle.StartDate,
                cycle.EndDate,
                CancellationToken.None));

        Assert.Equal(StatusCodes.Status409Conflict, exception.StatusCode);
    }

    [Fact]
    public async Task GetAsync_RejectsMissingCycle()
    {
        await using var context = CreateContext();
        var service = new PayrollCycleService(context);

        var exception = await Assert.ThrowsAsync<ApiException>(() =>
            service.GetAsync(Guid.NewGuid(), CancellationToken.None));

        Assert.Equal(StatusCodes.Status404NotFound, exception.StatusCode);
    }

    [Fact]
    public async Task ListAsync_OrdersCyclesByNewestStartDate()
    {
        await using var context = CreateContext();
        context.PayrollCycles.AddRange(
            NewCycle(new DateOnly(2026, 6, 1), new DateOnly(2026, 6, 14)),
            NewCycle(new DateOnly(2026, 7, 1), new DateOnly(2026, 7, 14)),
            NewCycle(new DateOnly(2026, 6, 15), new DateOnly(2026, 6, 28)));
        await context.SaveChangesAsync();
        var service = new PayrollCycleService(context);

        var cycles = await service.ListAsync(CancellationToken.None);

        Assert.Equal(
            [
                new DateOnly(2026, 7, 1),
                new DateOnly(2026, 6, 15),
                new DateOnly(2026, 6, 1)
            ],
            cycles.Select(cycle => cycle.StartDate));
    }

    private static FastPayDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<FastPayDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        return new FastPayDbContext(options);
    }

    private static PayrollCycle NewCycle(
        DateOnly startDate,
        DateOnly endDate,
        string status = PayrollCycleStatuses.Open)
    {
        var now = DateTimeOffset.UtcNow;

        return new PayrollCycle
        {
            Id = Guid.NewGuid(),
            StartDate = startDate,
            EndDate = endDate,
            Status = status,
            FinalizedAt = status == PayrollCycleStatuses.Finalized ? now : null,
            CreatedAt = now,
            UpdatedAt = now
        };
    }
}
