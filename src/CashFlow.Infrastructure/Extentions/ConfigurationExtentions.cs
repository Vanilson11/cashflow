using Microsoft.Extensions.Configuration;

namespace CashFlow.Infrastructure.Extentions;
public static class ConfigurationExtentions
{
    public static bool IsTestEnvironment(this IConfiguration configuration)
    {
        return configuration.GetValue<bool>("InMemoryTest");
    }
}
