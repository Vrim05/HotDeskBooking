using AutoMapper;
using HotDeskBooking.Models.Dto;
using HotDeskBooking.Models.Entity;
using HotDeskBooking.Queries.Desks.GetDeskById;
using HotDeskBooking.UnitTests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBooking.UnitTests.ServiceTests.Desks;
public class GetDeskByIdTests : IClassFixture<ServiceFixture>
{
    public ServiceFixture _serviceFixture { get; }
    public HotDeskBookingContext _context { get; set; }
    private readonly IMapper _mapper;

    public GetDeskByIdTests(ServiceFixture serviceFixture)
    {
        var options = new DbContextOptionsBuilder<HotDeskBookingContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;
        _context = new HotDeskBookingContext(options);
        _serviceFixture = serviceFixture;
        _mapper = serviceFixture.Mapper;
    }

    [Fact]
    public async Task GetDeskByIdAsync_ReturnsDesk()
    {
        // Arrange
        using var context = new HotDeskBookingContext(new DatabaseFixture()._options);
        var service = _serviceFixture.CreateDeskService(context);
        var location = await _serviceFixture.CreateLocationAsync(context);
        var desk = await _serviceFixture.CreateDeskAsync(location.Id, context);

        // Act
        var result = await service.GetByIdAsync(new GetDeskByIdQuery(desk.Id), new CancellationToken());

        // Assert
        Assert.NotNull(result);
        Assert.IsType<DeskDto>(result);
    }

    [Fact]
    public async Task GetDeskByIdAsync_ReturnsKeyNotFoundException()
    {
        // Arrange
        using var context = new HotDeskBookingContext(new DatabaseFixture()._options);
        var service = _serviceFixture.CreateDeskService(context);
        var nonExistentDeskId = 1;

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
        {
            await service.GetByIdAsync(new GetDeskByIdQuery(nonExistentDeskId), new CancellationToken());
        });

    }
}
