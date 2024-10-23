using HotDeskBooking.Commands.Booking.Create;
using HotDeskBooking.Commands.Booking.Delete;
using HotDeskBooking.Commands.Booking.Update;
using HotDeskBooking.Models.Requests;
using HotDeskBooking.Queries.Booking.GetBookingById;
using HotDeskBooking.Queries.Booking.GetFilteredBooking;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotDeskBooking.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class BookingController(ILogger<BookingController> logger, IMediator mediator) : Controller
{
    private readonly ILogger<BookingController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [Authorize]
    [HttpGet("GetById/{id}")]
    public async Task<IActionResult> GetById(int id, CancellationToken ct)
    {
        try
        {
            return Ok(await _mediator.Send(new GetBookingByIdQuery(id), ct));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [Authorize]
    [HttpGet("GetFilteredBookings")]
    public async Task<IActionResult> GetFilteredBookings(int? userId, int? deskId, int? locationId, CancellationToken ct)
    {
        try
        {
            return Ok(await _mediator.Send(new GetFilteredBookingsQuery { DeskId = deskId, UserId = userId, LocationId = locationId}, ct));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [Authorize]
    [HttpPost("CreateBooking")]
    public async Task<IActionResult> CreateBookingAsync(CreateBookingRequest request, CancellationToken ct)
    {
        try
        {
            return Ok(await _mediator.Send(new CreateBookingCommand(request), ct));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [Authorize]
    [HttpPut("UpdateBooking")]
    public async Task<IActionResult> UpdateBookingAsync(UpdateBookingRequest request, CancellationToken ct)
    {
        try
        {
            return Ok(await _mediator.Send(new UpdateBookingCommand(request), ct));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [Authorize]
    [HttpDelete("DeleteBooking/{id}")]
    public async Task<IActionResult> DeleteBookingAsync(int id, CancellationToken ct)
    {
        try
        {
            return Ok(await _mediator.Send(new DeleteBookingCommand(id), ct));
        }
        catch (Exception)
        {
            throw;
        }
    }
}