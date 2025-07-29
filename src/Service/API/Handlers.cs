namespace Innago.Public.NotificationTemplater.API;

using System.Text.Json;

using HandlerModels;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

using Platform.Messaging.EntityEvents;
using Platform.Messaging.Publisher;

using Scriban;

using Shared.TryHelpers;

[PublicAPI]
public static class Handlers
{
    [Pure]
    public static string Generate([FromBody] GenerateInput input)
    {
        Template? template = Template.Parse(input.Template);

        object data = GetData(input.Model);
        string result = template?.Render(data, member => $"{char.ToLower(member.Name[0])}{member.Name[1..]}") ?? string.Empty;
        return result;
    }

    [MustUseReturnValue]
    public static async Task<string> GenerateFromSavedTemplateAsync(
        [FromBody] GenerateFromSavedTemplateInput input,
        [FromServices] IDistributedCache cache,
        CancellationToken cancellationToken
    )
    {
        string template = await cache.GetStringAsync(input.Key, cancellationToken).ConfigureAwait(false) ?? string.Empty;

        return Generate(new GenerateInput(template, input.Model));
    }

    public static async Task SaveTemplateAsync(
        [FromBody] TemplateSaveInput input,
        [FromServices] IDistributedCache cache,
        [FromServices] IPublisher publisher,
        [FromServices] ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        await cache.SetStringAsync(input.Key, input.Template, cancellationToken);

        Result publishResult = await TryHelpers.TryAsync(() =>
            publisher.PublishAsync(new EntityEventInfo<Template>(input.Key, Verb.Create), SourceGenerationContext.Default)).ConfigureAwait(false);

        publishResult.IfFailed(PublisherError);

        return;

        Unit PublisherError(Exception? exception)
        {
            ILogger logger = loggerFactory.CreateLogger(nameof(Handlers));
            logger.Error(exception);
            return Unit.Default;
        }
    }

    [Pure]
    private static object GetData(string json)
    {
        object data = new { };

        try
        {
            data = (object?)JsonSerializer.Deserialize<JsonElement>(json) ?? new { };
        }
        catch
        {
            // do nothing
        }

        return data;
    }
}