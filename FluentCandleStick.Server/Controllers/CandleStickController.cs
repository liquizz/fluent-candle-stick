using FluentCandleStick.Application.Aggregates.CandleStick.Queries;
using FluentCandleStick.Application.Aggregates.MarketData.Commands;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FluentCandleStick.Server.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CandleStickController(ISender mediator) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetCandleStickData()
    {
        var result = await mediator.Send(new GetCandleStickDataQuery());
        return Ok(result);
    }

    [HttpPost("import")]
    public async Task<IActionResult> ImportCsvData(IFormFile file)
    {
        if (file == null || file.Length == 0)
            return BadRequest("No file uploaded");

        await using var stream = file.OpenReadStream();
        var result = await mediator.Send(new ImportMarketDataFromCsvCommand(stream));

        if (result)
            return Ok("Data imported successfully");
        else
            return BadRequest("Failed to import data");
    }
}