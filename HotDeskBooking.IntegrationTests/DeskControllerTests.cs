using HotDeskBooking.IntegrationTests.Helpers;
using HotDeskBooking.Models.Dto;
using HotDeskBooking.Models.Entity;
using HotDeskBooking.Models.Responses;
using Newtonsoft.Json;
using System.Net;

namespace HotDeskBooking.IntegrationTests;

public class DeskControllerTests : IClassFixture<WebAppFactory>
{
    private WebAppFactory _factory;
    private HttpClient _client;

    public DeskControllerTests(WebAppFactory factory)
    {
        _factory = factory;
        _client = factory.CreateClient();
        _client.BaseAddress = new Uri("https://localhost/");
    }

    [Fact]
    public async Task GetDeskByIdAsync_ReturnsOkResult()
    {
        ContextHelper.CreateDbContext(_factory, out HotDeskBookingContext db);
        var location = await CreateLocationAsync(db);
        var desk = await CreateDeskAsync(location.Id, db);

        try
        {
            var response = await _client.GetAsync($"v1/Desks/GetDeskById/{desk.Id}");
            var content = JsonConvert.DeserializeObject<DeskDto>(await response.Content.ReadAsStringAsync());
            content.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        finally
        {
            db.Remove(location);
            db.Remove(desk);
            await db.SaveChangesAsync();
        }
    }

    [Fact]
    public async Task GetFilteredDesksAsync_ReturnsOkResult()
    {
        ContextHelper.CreateDbContext(_factory, out HotDeskBookingContext db);
        var location = await CreateLocationAsync(db);
        var desk1 = await CreateDeskAsync(location.Id, db);
        var desk2 = await CreateDeskAsync(location.Id, db);

        try
        {
            var response = await _client.GetAsync($"v1/Desks/GetFilteredDesks");
            var content = JsonConvert.DeserializeObject<GetFilteredDesksResponse>(await response.Content.ReadAsStringAsync());
            content.Should().NotBeNull();
            response.StatusCode.Should().Be(HttpStatusCode.OK);
        }
        finally
        {
            db.Remove(location);
            db.Remove(desk1);
            db.Remove(desk2);
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

    private async Task<Desk> CreateDeskAsync(int locationId, HotDeskBookingContext context)
    {
        var entity = new Desk()
        {
            LocationId = locationId,
        };

        await context.AddAsync(entity);
        await context.SaveChangesAsync();
        return entity;
    }
}