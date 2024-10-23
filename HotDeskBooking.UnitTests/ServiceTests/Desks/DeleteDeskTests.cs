using AutoMapper;
using HotDeskBooking.Commands.Desks.Delete;
using HotDeskBooking.Commands.Locations.Delete;
using HotDeskBooking.Models.Entity;
using HotDeskBooking.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBooking.UnitTests.ServiceTests.Desks;
public class DeleteDeskTests : IClassFixture<ServiceFixture>
{
    public ServiceFixture _serviceFixture { get; }
    public HotDeskBookingContext _context { get; set; }
    private readonly IMapper _mapper;

    public DeleteDeskTests(ServiceFixture serviceFixture)
    {
        var options = new DbContextOptionsBuilder<HotDeskBookingContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new HotDeskBookingContext(options);
        _serviceFixture = serviceFixture;
        _mapper = serviceFixture.Mapper;
    }

    [Fact]
    public async Task DeleteDeskAsync_ReturnsSuccess()
    {
        // Arrange
        using var context = new HotDeskBookingContext(new DatabaseFixture()._options);
        var service = _serviceFixture.CreateDeskService(context);
        var location = await _serviceFixture.CreateLocationAsync(context);
        var desk = await _serviceFixture.CreateDeskAsync(location.Id, context);

        // Act
        var result = await service.DeleteAsync(new DeleteDeskCommand(desk.Id), new CancellationToken());

        // Assert
        Assert.NotNull(result);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task DeleteDeskAsync_ReturnsKeyNotFoundException()
    {
        // Arrange
        using var context = new HotDeskBookingContext(new DatabaseFixture()._options);
        var service = _serviceFixture.CreateDeskService(context);
        var nonExistentDeskId = 1;

        // Act
        var result = await service.DeleteAsync(new DeleteDeskCommand(nonExistentDeskId), new CancellationToken());

        //Assert
        Assert.NotNull(result.Error);
        Assert.False(result.Success);
    }

    [Fact]
    public async Task DeleteDeskAsync_ReturnsCannotRemoveIfBookingExist()
    {
        // Arrange
        using var context = new HotDeskBookingContext(new DatabaseFixture()._options);
        var service = _serviceFixture.CreateDeskService(context);
        var location = await _serviceFixture.CreateLocationAsync(context);
        var desk = await _serviceFixture.CreateDeskAsync(location.Id, context);
        var user = await _serviceFixture.CreateUserAsync(context);
        var booking = await _serviceFixture.CreateBookingAsync(desk.Id, user.Id, context);


        // Act
        var result = await service.DeleteAsync(new DeleteDeskCommand(desk.Id), new CancellationToken());

        // Assert
        Assert.NotNull(result.Error);
        Assert.False(result.Success);
    }

}
