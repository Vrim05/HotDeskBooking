using AutoMapper;
using HotDeskBooking.Models.Dto;
using HotDeskBooking.Models.Entity;
using HotDeskBooking.Queries.Locations.GetLocationById;
using HotDeskBooking.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBooking.UnitTests.ServiceTests.Locations;
public class GetLocationByIdTests : IClassFixture<ServiceFixture>
{
    public ServiceFixture _serviceFixture { get; }
    public HotDeskBookingContext _context { get; set; }
    private readonly IMapper _mapper;

    public GetLocationByIdTests(ServiceFixture serviceFixture)
    {
        var options = new DbContextOptionsBuilder<HotDeskBookingContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new HotDeskBookingContext(options);
        _serviceFixture = serviceFixture;
        _mapper = serviceFixture.Mapper;
    }

    [Fact]
    public async Task GetLocationByIdAsync_ReturnsLocation()
    {
        // Arrange
        using var context = new HotDeskBookingContext(new DatabaseFixture()._options);
        var service = _serviceFixture.CreateLocationService(context);
        var location = await _serviceFixture.CreateLocationAsync(context);

        // Act
        var result = await service.GetByIdAsync(new GetLocationByIdQuery(location.Id), new CancellationToken());

        // Assert
        Assert.NotNull(result);
        Assert.IsType<LocationDto>(result);
    }

    [Fact]
    public async Task GetLocationByIdAsync_ReturnsKeyNotFoundException()
    {
        // Arrange
        using var context = new HotDeskBookingContext(new DatabaseFixture()._options);
        var service = _serviceFixture.CreateLocationService(context);
        var nonExistentLocationId = 1;

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
        {
            await service.GetByIdAsync(new GetLocationByIdQuery(nonExistentLocationId), new CancellationToken());
        });

    }
}
