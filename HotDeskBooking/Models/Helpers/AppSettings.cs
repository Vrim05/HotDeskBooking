namespace HotDeskBooking.Models.Helpers;

public class AppSettings
{
    public static string Secret { get; set; }
    public static string JwtExpireMinutes { get; set; }
    public static string RefreshExpireDays { get; set; }

    static AppSettings()
    {
        IConfigurationRoot configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        Secret = configuration["Auth:Secret"];
        JwtExpireMinutes = configuration["Auth:JwtExpireMinutes"];
        RefreshExpireDays = configuration["Auth:RefreshExpireDays"];
    }
}
