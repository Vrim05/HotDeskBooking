using AutoMapper;
using HotDeskBooking.Commands.Locations.Delete;
using HotDeskBooking.Models.Entity;
using HotDeskBooking.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBooking.UnitTests.ServiceTests.Locations;
public class DeleteLocationTests : IClassFixture<ServiceFixture>
{
    public ServiceFixture _serviceFixture { get; }
    public HotDeskBookingContext _context { get; set; }
    private readonly IMapper _mapper;

    public DeleteLocationTests(ServiceFixture serviceFixture)
    {
        var options = new DbContextOptionsBuilder<HotDeskBookingContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new HotDeskBookingContext(options);
        _serviceFixture = serviceFixture;
        _mapper = serviceFixture.Mapper;
    }

    [Fact]
    public async Task DeleteLocationAsync_ReturnsSuccess()
    {
        // Arrange
        using var context = new HotDeskBookingContext(new DatabaseFixture()._options);
        var service = _serviceFixture.CreateLocationService(context);
        var location = await _serviceFixture.CreateLocationAsync(context);

        // Act
        var result = await service.DeleteAsync(new DeleteLocationCommand(location.Id), new CancellationToken());

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task DeleteLocationAsync_ReturnsKeyNotFoundException()
    {
        // Arrange
        using var context = new HotDeskBookingContext(new DatabaseFixture()._options);
        var service = _serviceFixture.CreateLocationService(context);
        var nonExistentLocationId = 1;

        // Act
        var result = await service.DeleteAsync(new DeleteLocationCommand(nonExistentLocationId), new CancellationToken());

        //Assert
        Assert.NotNull(result.Error);
        Assert.False(result.Success);
    }

    [Fact]
    public async Task DeleteLocationAsync_ReturnsCannotRemoveIfDeskExist()
    {
        // Arrange
        using var context = new HotDeskBookingContext(new DatabaseFixture()._options);
        var service = _serviceFixture.CreateLocationService(context);
        var location = await _serviceFixture.CreateLocationAsync(context);
        var desk = await _serviceFixture.CreateDeskAsync(location.Id, context);

        // Act
        var result = await service.DeleteAsync(new DeleteLocationCommand(location.Id), new CancellationToken());

        // Assert
        Assert.NotNull(result.Error);
        Assert.False(result.Success);
    }

}
