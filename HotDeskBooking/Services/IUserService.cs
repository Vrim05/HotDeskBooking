using HotDeskBooking.Models.Dto;
using HotDeskBooking.Models.Responses;
using HotDeskBooking.Commands.Users.Create;
using HotDeskBooking.Commands.Users.Delete;
using HotDeskBooking.Queries.Users.GetUserById;
using HotDeskBooking.Commands.Users.Login;

namespace HotDeskBooking.Services;

public interface IUserService
{
    int? GetUserIdFromHttpContextIfExist();
    Task<AuthenticationResponse> AuthenticateAsync(LoginCommand model, CancellationToken ct);
    Task<IReadOnlyList<UserDto>> GetAllAsync(CancellationToken ct);
    Task<IReadOnlyList<UserRoleDto>> GetUserRolesAsync(CancellationToken ct);
    Task<UserDto> GetByIdAsync(GetUserByIdQuery query, CancellationToken ct);
    Task<CreateUserResponse> CreateAsync(CreateUserCommand command, CancellationToken ct);
    Task<StandardResponse> DeleteAsync(DeleteUserCommand command, CancellationToken ct);
}