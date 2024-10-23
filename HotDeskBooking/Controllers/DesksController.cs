using HotDeskBooking.Commands.Desks.Create;
using HotDeskBooking.Commands.Desks.Delete;
using HotDeskBooking.Models.Requests;
using HotDeskBooking.Queries.Desks.GetDeskById;
using HotDeskBooking.Queries.Desks.GetFilteredDesk;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskBooking.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class DesksController(ILogger<DesksController> logger, IMediator mediator) : Controller
{
    private readonly ILogger<DesksController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [HttpGet("GetDeskById/{id}")]
    public async Task<IActionResult> GetDeskById(int id, CancellationToken ct)
    {
        try
        {
            return Ok(await _mediator.Send(new GetDeskByIdQuery(id), ct));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("GetFilteredDesks")]
    public async Task<IActionResult> GetFilteredDesks(int? locationId, DateTime? startDay, DateTime? endDate, CancellationToken ct)
    {
        try
        {
            return Ok(await _mediator.Send(new GetFilteredDesksQuery() { LocationId = locationId, StartDay = startDay, EndDate = endDate}, ct));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [Authorize]
    [HttpPost("CreateDesk")]
    public async Task<IActionResult> CreateDeskAsync(CreateDeskRequest request, CancellationToken ct)
    {
        try
        {
            return Ok(await _mediator.Send(new CreateDeskCommand(request), ct));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [Authorize]
    [HttpDelete("DeleteDesk/{id}")]
    public async Task<IActionResult> DeleteDeskAsync(int id, CancellationToken ct)
    {
        try
        {
            return Ok(await _mediator.Send(new DeleteDeskCommand(id), ct));
        }
        catch (Exception)
        {
            throw;
        }
    }
}