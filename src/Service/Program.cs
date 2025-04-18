using System.Diagnostics.CodeAnalysis;

using Innago.Public.NotificationTemplater;

AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromSeconds(2));

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.ConfigureServices();

WebApplication app = builder.Build();
app.ConfigureRouting();

await app.RunAsync();

[ExcludeFromCodeCoverage]
internal static partial class Program;