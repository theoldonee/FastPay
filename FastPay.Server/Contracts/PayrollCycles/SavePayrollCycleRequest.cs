namespace FastPay.Server.Contracts.PayrollCycles;

public sealed record SavePayrollCycleRequest(DateOnly StartDate, DateOnly EndDate);
