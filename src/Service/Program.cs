using System.Text.Json;

using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi;

using Scriban;

AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromSeconds(2));

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi("v1",
    options => { options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0; });

WebApplication app = builder.Build();

app.MapOpenApi("/openapi.json").CacheOutput();

app.MapGet("/generate", Generate).WithDescription("Generates a string from a model and a template").WithTags("template");

await app.RunAsync();
return;

static string Generate([FromBody] GenerateInput input, CancellationToken cancellationToken)
{
    Template? template = Template.Parse(input.Template);
    object data = (object?)JsonSerializer.Deserialize<JsonElement>(input.Model) ?? new { };
    string result = template?.Render(data, member => $"{char.ToLower(member.Name[0])}{member.Name[1..]}") ?? string.Empty;
    return result;
}

internal record GenerateInput(string Template, string Model);