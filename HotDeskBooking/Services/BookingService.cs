using AutoMapper;
using HotDeskBooking.Commands.Booking.Create;
using HotDeskBooking.Commands.Booking.Delete;
using HotDeskBooking.Commands.Booking.Update;
using HotDeskBooking.Models.Dto;
using HotDeskBooking.Models.Entity;
using HotDeskBooking.Models.Enums;
using HotDeskBooking.Models.Responses;
using HotDeskBooking.Queries.Booking.GetBookingById;
using HotDeskBooking.Queries.Booking.GetFilteredBooking;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBooking.Services;

public class BookingService : IBookingService
{
    private readonly HotDeskBookingContext _context;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public BookingService(
        HotDeskBookingContext context,
        IUserService userService,
        IMapper mapper)
    {
        _context = context;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<BookingDto>> GetFilteredBookingsAsync(GetFilteredBookingsQuery query, CancellationToken ct)
    {
        await CheckUserPermissionsAsync(ct);

        var bookings = await _context.Bookings
            .Include(x => x.User)
                .ThenInclude(y => y.Role)
            .Include(b => b.Desk)
                .ThenInclude(d => d.Location)
            .Where(b =>
                (!query.UserId.HasValue || b.UserId == query.UserId.Value) &&
                (!query.DeskId.HasValue || b.DeskId == query.DeskId.Value) &&
                (!query.LocationId.HasValue || b.Desk.LocationId == query.LocationId.Value) &&
                !b.IsDeleted && !b.Desk.IsDeleted)
            .ToListAsync(cancellationToken: ct);

        return _mapper.Map<IReadOnlyList<BookingDto>>(bookings);
    }


    public async Task<BookingDto> GetByIdAsync(GetBookingByIdQuery query, CancellationToken ct)
    {
        await CheckUserPermissionsAsync(ct);

        var entity = await GetBookingAsync(query.Id, ct);

        return _mapper.Map<BookingDto>(entity);
    }

    public async Task<CreateOrUpdateBookingResponse> UpdateAsync(UpdateBookingCommand command, CancellationToken ct)
    {
        var entity = await GetBookingAsync(command.BookingRequest.Id, ct);

        if((entity.StartDay - DateTime.Now) < TimeSpan.FromHours(24))
            throw new InvalidOperationException("StartDay must be at least 24 hours in the future.");

        await CheckIfDeskExistAsync(command.BookingRequest.NewDeskId, ct);

        await CheckIfDeskIsOccupiedAsync(command.BookingRequest.NewDeskId, command.BookingRequest.NewStartDay, command.BookingRequest.NewEndDate, ct);

        await UserHasConflictingBookingAsync(entity.UserId, command.BookingRequest.NewStartDay, command.BookingRequest.NewEndDate, command.BookingRequest.Id, ct);

        entity.EndDate = command.BookingRequest.NewEndDate;
        entity.StartDay = command.BookingRequest.NewStartDay;
        entity.DeskId = command.BookingRequest.NewDeskId;

        _context.Bookings.Update(entity);
        await _context.SaveChangesAsync(ct);

        return new CreateOrUpdateBookingResponse
        {
            Success = true,
            Booking = _mapper.Map<BookingDto>(entity)
        };
    }

    public async Task<CreateOrUpdateBookingResponse> CreateAsync(CreateBookingCommand command, CancellationToken ct)
    {
        await CheckIfDeskIsOccupiedAsync(command.BookingRequest.DeskId, command.BookingRequest.StartDay, command.BookingRequest.EndDate, ct);

        await CheckIfUserHasBookingAsync(command.BookingRequest.UserId, command.BookingRequest.StartDay, command.BookingRequest.EndDate, ct);

        await CheckIfDeskExistAsync(command.BookingRequest.DeskId, ct);

        var entity = new Booking
        {
            UserId = command.BookingRequest.UserId,
            DeskId = command.BookingRequest.DeskId,
            StartDay = command.BookingRequest.StartDay,
            EndDate = command.BookingRequest.EndDate
        };

        await _context.Bookings.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);

        var result = await GetBookingAsync(entity.Id, ct);

        return new CreateOrUpdateBookingResponse
        {
            Success = true,
            Booking = _mapper.Map<BookingDto>(result)
        };
    }

    public async Task<StandardResponse> DeleteAsync(DeleteBookingCommand command, CancellationToken ct)
    {
        try
        {
            var entity = await GetBookingAsync(command.Id, ct);

            entity.IsDeleted = true;
            _context.Bookings.Update(entity);
            await _context.SaveChangesAsync(ct);
            return new StandardResponse
            {
                Success = true
            };
        }
        catch (Exception exception)
        {
            return new StandardResponse
            {
                Success = false,
                Error = exception.Message
            };
        }
    }

    #region Helper methods
    private async Task<Booking> GetBookingAsync(int id, CancellationToken ct)
    {
        var booking = await _context.Bookings
            .Include(x => x.User)
                .ThenInclude(y => y.Role)
            .Include(x => x.Desk)
                .ThenInclude(y => y.Location)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, cancellationToken: ct)
            ?? throw new KeyNotFoundException($"Booking with Id {id} not found.");

        return booking;
    }

    private async Task<Desk> GetDeskAsync(int id, CancellationToken ct)
    {
        var entity = await _context.Desks
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new KeyNotFoundException($"Desk with Id {id} not found.");

        return entity;
    }

    private async Task CheckIfUserHasBookingAsync(int userId, DateTime startDate, DateTime endDate, CancellationToken ct)
    {
        var hasBooking = await _context.Bookings
            .AnyAsync(b => b.UserId == userId &&
                           b.StartDay < endDate &&
                           b.EndDate > startDate && !b.IsDeleted, ct);

        if (hasBooking)
            throw new Exception($"The user already has a booking during the selected time period.");
    }

    private async Task CheckIfDeskIsOccupiedAsync(int deskId, DateTime startDate, DateTime endDate, CancellationToken ct)
    {
        var isOccupied = await _context.Bookings
            .AnyAsync(b => b.DeskId == deskId &&
                           b.StartDay < endDate &&
                           b.EndDate > startDate && !b.IsDeleted, ct);

        if (isOccupied)
            throw new Exception($"The desk is already booked for the selected time period.");
    }

    private async Task CheckIfDeskExistAsync(int deskId, CancellationToken ct)
    {
        var deskExist = await _context.Desks
            .AnyAsync(x => x.Id == deskId && !x.IsDeleted, ct);

        if (!deskExist)
            throw new Exception($"Desk with Id {deskId} not found.");
    }

    private async Task UserHasConflictingBookingAsync(int userId, DateTime startDay, DateTime endDate, int bookingIdToExclude, CancellationToken ct)
    {
        var entity = await _context.Bookings
            .AnyAsync(b => b.UserId == userId &&
                           b.StartDay < endDate &&
                           b.EndDate > startDay &&
                           b.Id != bookingIdToExclude &&
                           !b.IsDeleted, ct);

        if (entity)
            throw new Exception($"The user already has a booking during the selected time period.");
    }
    public async Task CheckUserPermissionsAsync(CancellationToken ct)
    {
        var userId = _userService.GetUserIdFromHttpContextIfExist();

        if (userId is null)
            throw new ArgumentNullException(nameof(userId), "User ID cannot be null.");

        var user = await _context.Users
            .FirstOrDefaultAsync(x => x.Id == userId, ct)
            ?? throw new KeyNotFoundException($"User with Id {userId} not found.");

        if (user.RoleId != (int)UserTypes.Admin)
            throw new InvalidOperationException("You do not have permission to perform this operation.");
    }

    #endregion
}
