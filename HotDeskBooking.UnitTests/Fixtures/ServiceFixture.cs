using AutoMapper;
using HotDeskBooking.Helpers;
using HotDeskBooking.Models.Entity;
using HotDeskBooking.Services;

namespace HotDeskBooking.UnitTests.Fixtures;
public class ServiceFixture
{
    public IMapper Mapper { get; private set; }

    public ServiceFixture()
    {
        Mapper = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile(new AutoProfile());
        }).CreateMapper();
    }

    public DeskService CreateDeskService(HotDeskBookingContext context)
        => new(context, Mapper);

    public LocationService CreateLocationService(HotDeskBookingContext context)
        => new(context, Mapper);

    public async Task<Desk> CreateDeskAsync(int locationId, HotDeskBookingContext context)
    {
        var entity = new Desk()
        {
            LocationId = locationId,
            
        };

        await context.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<Location> CreateLocationAsync(HotDeskBookingContext context)
    {
        var entity = new Location()
        {
            BuildNumber = 1,
            RoomNumber = 1,
            Name = "Location1"
        };

        await context.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<Booking> CreateBookingAsync(int deskId, int userId, HotDeskBookingContext context)
    {
        var entity = new Booking()
        {
            DeskId = deskId,
            UserId = userId,
            StartDay = DateTime.Now,
            EndDate = DateTime.Now.AddHours(8),
        };

        await context.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }

    public async Task<User> CreateUserAsync(HotDeskBookingContext context)
    {
        var entity = new User()
        {
            Email = "Admin@admin.com",
            PasswordHash = null,
            Role = new UserRole
            {
                Type = "test"
            }
        };

        await context.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}
