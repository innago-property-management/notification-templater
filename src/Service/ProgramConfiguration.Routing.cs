namespace Innago.Public.NotificationTemplater;

using Innago.Public.NotificationTemplater.API;

using Prometheus;

internal static partial class ProgramConfiguration
{
    public static partial void ConfigureRouting(this WebApplication app)
    {
        app.MapOpenApi("/openapi.json").CacheOutput();
        app.MapHealthChecks("/healthz");
        app.MapMetrics("/metricsz");

        app.MapGet("/generate", Handlers.Generate).WithDescription("Generates a string from a model and a template").WithTags("template");

        app.MapPost("/template", Handlers.SaveTemplateAsync).WithDescription("Saves a template").WithTags("template");

        app.MapGet("/generateFromSavedTemplate", Handlers.GenerateFromSavedTemplateAsync).WithDescription("Generates a string from a model and a saved template")
            .WithTags("template");
    }
}