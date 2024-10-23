using HotDeskBooking.IntegrationTests.Helpers;
using HotDeskBooking.Models.Dto;
using HotDeskBooking.Models.Entity;
using Newtonsoft.Json;
using System.Net;

namespace HotDeskBooking.IntegrationTests;

public class LocationControllerTests : IClassFixture<WebAppFactory>
{
    private WebAppFactory _factory;
    private HttpClient _client;

    public LocationControllerTests(WebAppFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _client.BaseAddress = new Uri("https://localhost/");
    }

    [Fact]
    public async Task GetLocationByIdAsync_ReturnsOkResult()
    {
        ContextHelper.CreateDbContext(_factory, out HotDeskBookingContext db);
        var entity = await CreateLocationAsync(db);

        try
        {
            var response = await _client.GetAsync($"v1/Locations/GetLocationById/{entity.Id}");
            var content = JsonConvert.DeserializeObject<LocationDto>(await response.Content.ReadAsStringAsync());
            content.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        finally
        {
            db.Remove(entity);
            await db.SaveChangesAsync();
        }

    }

    [Fact]
    public async Task GetLocationsAsync_ReturnsOkResult()
    {
        ContextHelper.CreateDbContext(_factory, out HotDeskBookingContext db);
        var entity1 = await CreateLocationAsync(db);
        var entity2 = await CreateLocationAsync(db);

        try
        {
            var response = await _client.GetAsync($"v1/Locations/GetLocations");
            var content = JsonConvert.DeserializeObject<IReadOnlyList<LocationDto>>(await response.Content.ReadAsStringAsync());
            content.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        finally
        {
            db.Remove(entity1);
            db.Remove(entity2);
            await db.SaveChangesAsync();
        }

    }

    private async Task<Location> CreateLocationAsync(HotDeskBookingContext context)
    {
        var entity = new Location()
        {
            BuildNumber = 1,
            RoomNumber = 1,
            Name = "Test"
        };

        await context.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}