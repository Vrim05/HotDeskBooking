using AutoMapper;
using HotDeskBooking.Commands.Desks.Create;
using HotDeskBooking.Commands.Desks.Delete;
using HotDeskBooking.Models.Dto;
using HotDeskBooking.Models.Entity;
using HotDeskBooking.Models.Responses;
using HotDeskBooking.Queries.Desks.GetDeskById;
using HotDeskBooking.Queries.Desks.GetFilteredDesk;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBooking.Services;

public class DeskService : IDeskService
{
    private readonly HotDeskBookingContext _context;
    private readonly IMapper _mapper;

    public DeskService(
        HotDeskBookingContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<GetFilteredDesksResponse> GetFilteredDesksAsync(GetFilteredDesksQuery query, CancellationToken ct)
    {
        List<int> bookedDeskIds = new List<int>();

        // Check if both StartDay and EndDate are provided
        if (query.StartDay.HasValue && query.EndDate.HasValue)
        {
            // Get the ids of desks that are booked in the given time slot
            bookedDeskIds = await _context.Bookings
                .Where(b => !b.IsDeleted &&
                (
                    !((b.StartDay > query.StartDay && b.StartDay >= query.EndDate) ||
                    (b.EndDate <= query.StartDay && b.EndDate < query.EndDate))
                ))
                .Select(b => b.DeskId)
                .ToListAsync(ct);
        }

        var availableDesks = await _context.Desks
            .Include(d => d.Location)
            .Where(d => (!query.LocationId.HasValue || d.LocationId == query.LocationId.Value) &&
                        !bookedDeskIds.Contains(d.Id) &&
                        !d.IsDeleted)
            .ToListAsync(ct);

        var unavailableDesks = await _context.Desks
            .Include(d => d.Location)
            .Where(d => (!query.LocationId.HasValue || d.LocationId == query.LocationId.Value) &&
                        bookedDeskIds.Contains(d.Id) &&
                        !d.IsDeleted)
            .ToListAsync(ct);

        var result = new GetFilteredDesksResponse
        {
            AvailableDesks = _mapper.Map<IReadOnlyList<DeskDto>>(availableDesks),
            UnavailableDesks = _mapper.Map<IReadOnlyList<DeskDto>>(unavailableDesks),
            Success = true
        };

        return result;
    }


    public async Task<DeskDto> GetByIdAsync(GetDeskByIdQuery query, CancellationToken ct)
    {
        var entity = await GetDeskAsync(query.Id, ct);

        return _mapper.Map<DeskDto>(entity);
    }

    public async Task<CreateDeskResponse> CreateAsync(CreateDeskCommand command, CancellationToken ct)
    {
        var location = await _context.Locations
            .FirstOrDefaultAsync(x => x.Id == command.DeskRequest.LocationId && !x.IsDeleted, ct)
            ?? throw new KeyNotFoundException($"Location with Id {command.DeskRequest.LocationId} not found.");

        var entity = new Desk
        {
            LocationId = command.DeskRequest.LocationId,
        };
        await _context.Desks.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);

        return new CreateDeskResponse
        {
            Success = true,
            Desk = _mapper.Map<DeskDto>(entity)
        };
    }

    public async Task<StandardResponse> DeleteAsync(DeleteDeskCommand command, CancellationToken ct)
    {
        try
        {
            var entity = await GetDeskAsync(command.Id, ct);

            var deskBooking = await _context.Bookings
                .Where(x => x.DeskId == entity.Id && !x.IsDeleted)
                .ToListAsync(ct);

            if (deskBooking.Any())
            {
                return new StandardResponse
                {
                    Success = false,
                    Error = "The desk cannot be deleted because there are reservations assigned to it."
                };
            }

            entity.IsDeleted = true;
            _context.Desks.Update(entity);
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
    private async Task<Desk> GetDeskAsync(int id, CancellationToken ct)
    {
        var entity = await _context.Desks
            .Include(x => x.Location)
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new KeyNotFoundException($"Desk with Id {id} not found.");

        return entity;
    }
    #endregion
}
