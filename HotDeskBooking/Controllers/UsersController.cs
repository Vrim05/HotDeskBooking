using HotDeskBooking.Commands.Users.Delete;
using HotDeskBooking.Commands.Users.Create;
using HotDeskBooking.Models.Requests;
using HotDeskBooking.Queries.Users.GetUserById;
using HotDeskBooking.Queries.Users.GetUsers;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotDeskBooking.Commands.Users.Login;
using HotDeskBooking.Queries.Users.GetUserRoles;

namespace HotDeskBooking.Controllers;

[ApiController]
[Route("v1/[controller]")]
public class UsersController(ILogger<UsersController> logger, IMediator mediator) : Controller
{
    private readonly ILogger<UsersController> _logger = logger;
    private readonly IMediator _mediator = mediator;

    [HttpPost("Authenticate")]
    public async Task<IActionResult> Authenticate([FromBody] LoginRequest command, CancellationToken ct)
    {
        try
        {
            return Ok(await _mediator.Send(new LoginCommand(command), ct));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [Authorize]
    [HttpGet("GetUserById/{id}")]
    public async Task<IActionResult> GetUserById(int id, CancellationToken ct)
    {
        try
        {
            return Ok(await _mediator.Send(new GetUserByIdQuery(id), ct));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [Authorize]
    [HttpGet("GetUserRoles")]
    public async Task<IActionResult> GetUserRoles(CancellationToken ct)
    {
        try
        {
            return Ok(await _mediator.Send(new GetUserRolesQuery(), ct));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [Authorize]
    [HttpGet("GetUsers")]
    public async Task<IActionResult> GetUsers(CancellationToken ct)
    {
        try
        {
            return Ok(await _mediator.Send(new GetUsersQuery(), ct));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [Authorize]
    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUserAsync(CreateUserRequest request, CancellationToken ct)
    {
        try
        {
            return Ok(await _mediator.Send(new CreateUserCommand(request), ct));
        }
        catch (Exception)
        {
            throw;
        }
    }

    [Authorize]
    [HttpDelete("DeleteUser/{id}")]
    public async Task<IActionResult> DeleteUserAsync(int id, CancellationToken ct)
    {
        try
        {
            return Ok(await _mediator.Send(new DeleteUserCommand(id), ct));
        }
        catch (Exception)
        {
            throw;
        }
    }
}