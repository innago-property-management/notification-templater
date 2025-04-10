namespace Innago.Public.NotificationTemplater.API;

using System.Text;
using System.Text.Json;

using Innago.Public.NotificationTemplater.API.HandlerModels;

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
        object data = (object?)JsonSerializer.Deserialize<JsonElement>(input.Model) ?? new { };
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
        byte[] data = await cache.GetAsync(input.Key, cancellationToken).ConfigureAwait(false) ?? [];

        string template = Encoding.UTF8.GetString(data);

        return Generate(new GenerateInput(template, input.Model));
    }

    public static Task SaveTemplateAsync(
        [FromBody] TemplateSaveInput input,
        [FromServices] IDistributedCache cache,
        CancellationToken cancellationToken)
    {
        return cache.SetStringAsync(input.Key, input.Template, cancellationToken);
    }
}