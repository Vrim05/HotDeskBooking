using AutoMapper;
using HotDeskBooking.Commands.Desks.Create;
using HotDeskBooking.Models.Entity;
using HotDeskBooking.Models.Requests;
using HotDeskBooking.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBooking.UnitTests.ServiceTests.Desks;
public class CreateDeskTests : IClassFixture<ServiceFixture>
{
    public ServiceFixture _serviceFixture { get; }
    public HotDeskBookingContext _context { get; set; }
    private readonly IMapper _mapper;

    public CreateDeskTests(ServiceFixture serviceFixture)
    {
        var options = new DbContextOptionsBuilder<HotDeskBookingContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new HotDeskBookingContext(options);
        _serviceFixture = serviceFixture;
        _mapper = serviceFixture.Mapper;
    }

    [Fact]
    public async Task CreateDeskAsync_ReturnsDesk()
    {
        // Arrange
        using var context = new HotDeskBookingContext(new DatabaseFixture()._options);
        var service = _serviceFixture.CreateDeskService(context);
        var location = await _serviceFixture.CreateLocationAsync(context);

        // Act
        var result = await service.CreateAsync(new CreateDeskCommand(new CreateDeskRequest() { LocationId = location.Id }), new CancellationToken());

        // Assert
        Assert.NotNull(result);
        Assert.NotNull(result.Desk);
        Assert.True(result.Success);
    }

    [Fact]
    public async Task CreateDeskAsync_ReturnsKeyNotFoundException()
    {
        // Arrange
        using var context = new HotDeskBookingContext(new DatabaseFixture()._options);
        var service = _serviceFixture.CreateDeskService(context);
        var nonExistentLocationId = 1;

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
        {
            await service.CreateAsync(new CreateDeskCommand(new CreateDeskRequest() { LocationId = nonExistentLocationId }), new CancellationToken());
        });
    }

}
