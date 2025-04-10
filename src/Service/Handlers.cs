namespace Service;

using System.Text;
using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

using Scriban;

internal static class Handlers
{
    internal static string Generate([FromBody] GenerateInput input)
    {
        Template? template = Template.Parse(input.Template);
        object data = (object?)JsonSerializer.Deserialize<JsonElement>(input.Model) ?? new { };
        string result = template?.Render(data, member => $"{char.ToLower(member.Name[0])}{member.Name[1..]}") ?? string.Empty;
        return result;
    }

    internal static async Task<string> GenerateFromSavedTemplateAsync(
        [FromBody] GenerateFromSavedTemplateInput input,
        [FromServices] IDistributedCache cache,
        CancellationToken cancellationToken
    )
    {
        byte[] data = await cache.GetAsync(input.Key, cancellationToken).ConfigureAwait(false) ?? [];

        string template = Encoding.UTF8.GetString(data);

        return Generate(new GenerateInput(template, input.Model));
    }

    internal static Task SaveTemplateAsync(
        [FromBody] TemplateSaveInput input,
        [FromServices] IDistributedCache cache,
        CancellationToken cancellationToken)
    {
        return cache.SetStringAsync(input.Key, input.Template, cancellationToken);
    }
}