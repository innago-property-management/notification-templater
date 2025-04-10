namespace Service;

using Microsoft.Extensions.Caching.StackExchangeRedis;

internal static partial class ProgramConfiguration
{
    private static Action<RedisCacheOptions> ConfigureValkey(string? configuration)
    {
        return Configure;

        void Configure(RedisCacheOptions options)
        {
            options.Configuration = configuration;
        }
    }
}