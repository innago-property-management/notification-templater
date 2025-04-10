namespace Innago.Public.NotificationTemplater;

using Prometheus;

internal static partial class ProgramConfiguration
{
    public static partial void ConfigureServices(this WebApplicationBuilder builder)
    {
        string serviceName = builder.Configuration["serviceName"]!;
        string serviceVersion = builder.Configuration["serviceVersion"] ?? "0.0.1";

        builder.Services.AddOpenApi("v1", ConfigureOpenApi);

        builder.Services.AddStackExchangeRedisCache(ConfigureValkey(builder.Configuration["valkey"]));

        builder.Services.AddMetrics();
        builder.Services.AddHealthChecks().ForwardToPrometheus();

        builder.Services.AddLogging(ConfigureLogging(builder.Configuration, serviceName));

        builder.Services.AddOpenTelemetry()
            .ConfigureResource(ConfigureResource(serviceName, serviceVersion))
            .WithTracing(ConfigureTracing(builder.Configuration, serviceName))
            .WithMetrics(ConfigureMetrics(serviceName));
    }
}