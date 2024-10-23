using AutoMapper;
using HotDeskBooking.Models.Dto;
using HotDeskBooking.Models.Entity;
using HotDeskBooking.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBooking.UnitTests.ServiceTests.Locations;
public class GetLocationsTests : IClassFixture<ServiceFixture>
{
    public ServiceFixture _serviceFixture { get; }
    public HotDeskBookingContext _context { get; set; }
    private readonly IMapper _mapper;

    public GetLocationsTests(ServiceFixture serviceFixture)
    {
        var options = new DbContextOptionsBuilder<HotDeskBookingContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new HotDeskBookingContext(options);
        _serviceFixture = serviceFixture;
        _mapper = serviceFixture.Mapper;
    }

    [Fact]
    public async Task GetLocationsAsync_ReturnsLocation()
    {
        // Arrange
        using var context = new HotDeskBookingContext(new DatabaseFixture()._options);
        var service = _serviceFixture.CreateLocationService(context);
        var location = await _serviceFixture.CreateLocationAsync(context);

        // Act
        var result = await service.GetLocationsAsync(new CancellationToken());

        // Assert
        Assert.NotNull(result);
        Assert.IsAssignableFrom<IReadOnlyList<LocationDto>>(result);
        Assert.Single(result);
    }
}
