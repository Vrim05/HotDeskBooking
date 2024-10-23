using System.Security.Cryptography;

namespace HotDeskBooking.Helpers.SignatureGenerator;

public static class SignatureGenerator
{
    public static string Generate()
    {
        using var rng = RandomNumberGenerator.Create();

        var bytes = new byte[120];
        rng.GetBytes(bytes);
        var signature = Convert.ToBase64String(bytes)
            .Replace('+', '_')
            .Replace('/', '_');

        return signature;
    }
}
