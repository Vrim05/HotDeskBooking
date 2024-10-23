using Newtonsoft.Json;
using System.Text;

namespace HotDeskBooking.IntegrationTests.Helpers;
internal static class HttpContentHelper
{
    public static HttpContent ToJsonHttpContent(this object obj)
    {
        return new StringContent(
            JsonConvert.SerializeObject(obj),
            Encoding.UTF8,
            "application/json"
        );
    }
}
