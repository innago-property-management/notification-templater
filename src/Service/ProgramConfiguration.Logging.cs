namespace Innago.Public.NotificationTemplater;

using OpenTelemetry.Resources;

using Serilog;
using Serilog.Core;

internal static partial class ProgramConfiguration
{
    private static Action<ILoggingBuilder> ConfigureLogging(IConfiguration configuration, string serviceName)
    {
        return Configure;

        void Configure(ILoggingBuilder loggingBuilder)
        {
            Logger serilog = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            loggingBuilder.AddSerilog(serilog);

            loggingBuilder.AddOpenTelemetry(options => { options.SetResourceBuilder(ResourceBuilder.CreateDefault().AddService(serviceName)); });
        }
    }
}