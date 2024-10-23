using HotDeskBooking.Models.Entity;
using Microsoft.Extensions.DependencyInjection;

namespace HotDeskBooking.IntegrationTests.Helpers;
internal class ContextHelper
{
    static internal void CreateDbContext(WebAppFactory factory, out HotDeskBookingContext db)
    {
        var scopeFactory = factory.Services.GetService<IServiceScopeFactory>() ?? throw new Exception();
        IServiceScope scope = scopeFactory.CreateScope();

        db = scope.ServiceProvider.GetService<HotDeskBookingContext>();
    }
}
