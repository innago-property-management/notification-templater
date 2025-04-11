namespace Innago.Public.NotificationTemplater;

using System.Diagnostics.CodeAnalysis;

[ExcludeFromCodeCoverage]
internal static partial class ProgramConfiguration
{
    public static partial void ConfigureRouting(this WebApplication app);
    public static partial void ConfigureServices(this WebApplicationBuilder builder);
}