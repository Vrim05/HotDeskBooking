using HotDeskBooking.Models.Entity;
using Microsoft.EntityFrameworkCore;

namespace HotDeskBooking.UnitTests.Fixtures;
public class DatabaseFixture : IAsyncLifetime
{
    public readonly DbContextOptions<HotDeskBookingContext> _options;
    private readonly string _databaseName;
    public DatabaseFixture()
    {
        _databaseName = Guid.NewGuid().ToString();
        _options = new DbContextOptionsBuilder<HotDeskBookingContext>()
            .UseInMemoryDatabase(databaseName: _databaseName)
            .Options;
    }
    public async Task DisposeAsync()
    {
        using var context = new HotDeskBookingContext(_options);
        await context.Database.EnsureDeletedAsync();
    }
    public async Task InitializeAsync()
    {
        using var context = new HotDeskBookingContext(_options);
        await context.Database.EnsureCreatedAsync();
    }
}
