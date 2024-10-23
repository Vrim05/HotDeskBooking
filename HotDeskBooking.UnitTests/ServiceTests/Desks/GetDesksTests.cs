using AutoMapper;
using HotDeskBooking.Models.Dto;
using HotDeskBooking.Models.Entity;
using HotDeskBooking.Queries.Desks.GetFilteredDesk;
using HotDeskBooking.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBooking.UnitTests.ServiceTests.Desks;
public class GetFilteredDesksTests : IClassFixture<ServiceFixture>
{
    public ServiceFixture _serviceFixture { get; }
    public HotDeskBookingContext _context { get; set; }
    private readonly IMapper _mapper;

    public GetFilteredDesksTests(ServiceFixture serviceFixture)
    {
        var options = new DbContextOptionsBuilder<HotDeskBookingContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new HotDeskBookingContext(options);
        _serviceFixture = serviceFixture;
        _mapper = serviceFixture.Mapper;
    }

    [Fact]
    public async Task GetFilteredDesksAsync_ReturnsAvailableDesksInLocation()
    {
        // Arrange
        using var context = new HotDeskBookingContext(new DatabaseFixture()._options);
        var service = _serviceFixture.CreateDeskService(context);
        var location = await _serviceFixture.CreateLocationAsync(context);
        var desk1 = await _serviceFixture.CreateDeskAsync(location.Id, context);
        var desk2 = await _serviceFixture.CreateDeskAsync(location.Id, context);

        // Act
        var result = await service.GetFilteredDesksAsync(new GetFilteredDesksQuery { LocationId = location.Id }, new CancellationToken());

        // Assert
        Assert.NotNull(result.AvailableDesks);
        Assert.IsAssignableFrom<IReadOnlyList<DeskDto>>(result.AvailableDesks);
        Assert.Equal(2, result.AvailableDesks.Count);
    }

    [Fact]
    public async Task GetFilteredDesksAsync_ReturnsAvailableDesksForDay()
    {
        // Arrange
        using var context = new HotDeskBookingContext(new DatabaseFixture()._options);
        var service = _serviceFixture.CreateDeskService(context);
        var location = await _serviceFixture.CreateLocationAsync(context);
        var desk1 = await _serviceFixture.CreateDeskAsync(location.Id, context);
        var desk2 = await _serviceFixture.CreateDeskAsync(location.Id, context);

        // Act
        var result = await service.GetFilteredDesksAsync(new GetFilteredDesksQuery {LocationId = location.Id, StartDay = DateTime.Now, EndDate = DateTime.Now.AddHours(8) }, new CancellationToken());

        // Assert
        Assert.NotNull(result.AvailableDesks);
        Assert.IsAssignableFrom<IReadOnlyList<DeskDto>>(result.AvailableDesks);
        Assert.Equal(2, result.AvailableDesks.Count);
    }

    [Fact]
    public async Task GetFilteredDesksAsync_ReturnsUnavailableDesks()
    {
        // Arrange
        using var context = new HotDeskBookingContext(new DatabaseFixture()._options);
        var service = _serviceFixture.CreateDeskService(context);
        var location = await _serviceFixture.CreateLocationAsync(context);
        var user = await _serviceFixture.CreateUserAsync(context);
        var desk1 = await _serviceFixture.CreateDeskAsync(location.Id, context);
        var desk2 = await _serviceFixture.CreateDeskAsync(location.Id, context);
        var booking1 = await _serviceFixture.CreateBookingAsync(desk1.Id, user.Id, context); ;

        // Act
        var result = await service.GetFilteredDesksAsync(new GetFilteredDesksQuery { LocationId = location.Id, StartDay = DateTime.Now, EndDate = DateTime.Now.AddHours(8) }, new CancellationToken());

        // Assert
        Assert.NotNull(result.UnavailableDesks);
        Assert.NotNull(result.AvailableDesks);
        Assert.IsAssignableFrom<IReadOnlyList<DeskDto>>(result.AvailableDesks);
        Assert.IsAssignableFrom<IReadOnlyList<DeskDto>>(result.UnavailableDesks);
        Assert.Single(result.UnavailableDesks);
        Assert.Single(result.UnavailableDesks);
    }
}
