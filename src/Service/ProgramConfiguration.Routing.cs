namespace Innago.Public.NotificationTemplater;

using API;

using Innago.Public.NotificationTemplater.API.Service;

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

        app.MapGet("/generateFromSavedTemplate", Handlers.GenerateFromSavedTemplateAsync)
            .WithDescription("Generates a string from a model and a saved template")
            .WithTags("template");

        // New endpoint to get a template by key
        app.MapGet("/template/{key}", Handlers.GetTemplateAsync)
            .WithDescription("Retrieves a saved template by its key")
            .WithTags("template");

        // Map your gRPC service
        app.MapGrpcService<NotificationTemplaterService>(); // Uses the gRPC service from API.Services

        // Optional: A fallback for non-gRPC requests to the gRPC port
        app.MapGet("/", () => "Communication with gRPC endpoints must be made through a gRPC client.");
    }
}