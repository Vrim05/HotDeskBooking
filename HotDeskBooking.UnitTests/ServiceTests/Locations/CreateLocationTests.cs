using AutoMapper;
using HotDeskBooking.Commands.Locations.Create;
using HotDeskBooking.Models.Entity;
using HotDeskBooking.Models.Requests;
using HotDeskBooking.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBooking.UnitTests.ServiceTests.Locations;
public class CreateLocationTests : IClassFixture<ServiceFixture>
{
    public ServiceFixture _serviceFixture { get; }
    public HotDeskBookingContext _context { get; set; }
    private readonly IMapper _mapper;

    public CreateLocationTests(ServiceFixture serviceFixture)
    {
        var options = new DbContextOptionsBuilder<HotDeskBookingContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new HotDeskBookingContext(options);
        _serviceFixture = serviceFixture;
        _mapper = serviceFixture.Mapper;
    }

    [Fact]
    public async Task CreateLocationAsync_ReturnsLocation()
    {
        // Arrange
        using var context = new HotDeskBookingContext(new DatabaseFixture()._options);
        var service = _serviceFixture.CreateLocationService(context);

        // Act
        var result = await service.CreateAsync(new CreateLocationCommand(new CreateLocationRequest() { BuildNumber = 1, RoomNumber = 1, Name = "test" }), new CancellationToken());

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Location);
        Assert.True(result.Success);
    }

}
