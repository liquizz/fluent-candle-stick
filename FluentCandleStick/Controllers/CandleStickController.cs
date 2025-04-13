using FluentCandleStick.Application.Aggregates.CandleStick.Queries;
using FluentCandleStick.Application.Aggregates.MarketData.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FluentCandleStick.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CandleStickController : ControllerBase
{
    private readonly IMediator _mediator;

    public CandleStickController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetCandleStickData()
    {
        var result = await _mediator.Send(new GetCandleStickDataQuery());
        return Ok(result);
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportCsvData(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        using var stream = file.OpenReadStream();
        var result = await _mediator.Send(new ImportMarketDataFromCsvCommand(stream));

        if (result)
            return Ok("Data imported successfully");
        else
            return BadRequest("Failed to import data");
    }
}