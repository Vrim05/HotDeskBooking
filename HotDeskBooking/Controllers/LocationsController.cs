using HotDeskBooking.Commands.Locations.Create;
using HotDeskBooking.Commands.Locations.Delete;
using HotDeskBooking.Models.Requests;
using HotDeskBooking.Queries.Locations.GetLocationById;
using HotDeskBooking.Queries.Locations.GetLocations;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskBooking.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class LocationsController(ILogger<LocationsController> logger, IMediator mediator) : Controller
{
    private readonly ILogger<LocationsController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [HttpGet("GetLocationById/{id}")]
    public async Task<IActionResult> GetLocationById(int id, CancellationToken ct)
    {
        try
        {
            return Ok(await _mediator.Send(new GetLocationByIdQuery(id), ct));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [HttpGet("GetLocations")]
    public async Task<IActionResult> GetLocations(CancellationToken ct)
    {
        try
        {
            return Ok(await _mediator.Send(new GetLocationsQuery(), ct));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [Authorize]
    [HttpPost("CreateLocation")]
    public async Task<IActionResult> CreateLocationAsync(CreateLocationRequest request, CancellationToken ct)
    {
        try
        {
            return Ok(await _mediator.Send(new CreateLocationCommand(request), ct));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [Authorize]
    [HttpDelete("DeleteLocation/{id}")]
    public async Task<IActionResult> DeleteLocationAsync(int id, CancellationToken ct)
    {
        try
        {
            return Ok(await _mediator.Send(new DeleteLocationCommand(id), ct));
        }
        catch (Exception)
        {
            throw;
        }
    }
}