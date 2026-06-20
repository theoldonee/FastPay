using FastPay.Server.Contracts.PayrollCycles;
using FastPay.Server.Services;
using Microsoft.AspNetCore.Mvc;

namespace FastPay.Server.Controllers;

[ApiController]
[Route("api/payroll-cycles")]
public sealed class PayrollCyclesController(PayrollCycleService payrollCycleService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<PayrollCycleResponse>>> List(
        CancellationToken cancellationToken)
    {
        var cycles = await payrollCycleService.ListAsync(cancellationToken);
        return Ok(cycles.Select(PayrollCycleResponse.FromModel));
    }

    [HttpGet("{cycleId:guid}")]
    public async Task<ActionResult<PayrollCycleResponse>> Get(
        Guid cycleId,
        CancellationToken cancellationToken)
    {
        var cycle = await payrollCycleService.GetAsync(cycleId, cancellationToken);
        return Ok(PayrollCycleResponse.FromModel(cycle));
    }

    [HttpPost]
    public async Task<ActionResult<PayrollCycleResponse>> Create(
        SavePayrollCycleRequest request,
        CancellationToken cancellationToken)
    {
        var cycle = await payrollCycleService.CreateAsync(
            request.StartDate,
            request.EndDate,
            cancellationToken);
        var response = PayrollCycleResponse.FromModel(cycle);

        return CreatedAtAction(nameof(Get), new { cycleId = cycle.Id }, response);
    }

    [HttpPut("{cycleId:guid}")]
    public async Task<ActionResult<PayrollCycleResponse>> Update(
        Guid cycleId,
        SavePayrollCycleRequest request,
        CancellationToken cancellationToken)
    {
        var cycle = await payrollCycleService.UpdateAsync(
            cycleId,
            request.StartDate,
            request.EndDate,
            cancellationToken);

        return Ok(PayrollCycleResponse.FromModel(cycle));
    }
}
