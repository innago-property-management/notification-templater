namespace Innago.Public.NotificationTemplater.API;

using System.Text.Json;

using HandlerModels;

using JetBrains.Annotations;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

using Scriban;

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

    public static Task SaveTemplateAsync(
        [FromBody] TemplateSaveInput input,
        [FromServices] IDistributedCache cache,
        CancellationToken cancellationToken)
    {
        return cache.SetStringAsync(input.Key, input.Template, cancellationToken);
    }
    // New method to get a template by key
    [MustUseReturnValue]
    public static async Task<string> GetTemplateAsync(
        [FromRoute] string key, // Use [FromRoute] to bind the key from the URL path
        [FromServices] IDistributedCache cache,
        CancellationToken cancellationToken
    )
    {
        string template = await cache.GetStringAsync(key, cancellationToken).ConfigureAwait(false) ?? string.Empty;
        return template;
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