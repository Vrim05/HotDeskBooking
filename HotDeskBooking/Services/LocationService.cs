using AutoMapper;
using HotDeskBooking.Commands.Locations.Create;
using HotDeskBooking.Commands.Locations.Delete;
using HotDeskBooking.Models.Dto;
using HotDeskBooking.Models.Entity;
using HotDeskBooking.Models.Responses;
using HotDeskBooking.Queries.Locations.GetLocationById;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBooking.Services;

public class LocationService : ILocationService
{
    private readonly HotDeskBookingContext _context;
    private readonly IMapper _mapper;

    public LocationService(
        HotDeskBookingContext context,
        IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IReadOnlyList<LocationDto>> GetLocationsAsync(CancellationToken ct)
    {
        var locations = await _context.Locations
            .Where(l => !l.IsDeleted)
            .ToListAsync(cancellationToken: ct);

        return _mapper.Map<IReadOnlyList<LocationDto>>(locations);
    }


    public async Task<LocationDto> GetByIdAsync(GetLocationByIdQuery query, CancellationToken ct)
    {
        var entity = await GetLocationAsync(query.Id, ct);

        return _mapper.Map<LocationDto>(entity);
    }

    public async Task<CreateLocationResponse> CreateAsync(CreateLocationCommand command, CancellationToken ct)
    {
        var entity = new Location
        {
            BuildNumber = command.CreateRequest.BuildNumber,
            RoomNumber = command.CreateRequest.RoomNumber,
            Name = command.CreateRequest.Name
        };
        await _context.Locations.AddAsync(entity, ct);
        await _context.SaveChangesAsync(ct);

        return new CreateLocationResponse
        {
            Success = true,
            Location = _mapper.Map<LocationDto>(entity)
        };
    }

    public async Task<StandardResponse> DeleteAsync(DeleteLocationCommand command, CancellationToken ct)
    {
        try
        {
            var entity = await GetLocationAsync(command.Id, ct);

            var locationDesks = await _context.Desks
                .Where(x => x.LocationId == entity.Id && !x.IsDeleted)
                .ToListAsync(ct);

            if (locationDesks.Any())
            {
                return new StandardResponse
                {
                    Success = false,
                    Error = "Cannot delete location because there are desks assigned to it."
                };
            }

            entity.IsDeleted = true;
            _context.Locations.Update(entity);
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
    private async Task<Location> GetLocationAsync(int id, CancellationToken ct)
    {
        var location = await _context.Locations
            .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted, ct)
            ?? throw new KeyNotFoundException($"Location with Id {id} not found.");

        return location;
    }
    #endregion
}