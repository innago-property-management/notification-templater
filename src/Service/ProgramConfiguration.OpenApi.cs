namespace Service;

using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

internal static partial class ProgramConfiguration
{
    private static void ConfigureOpenApi(OpenApiOptions options)
    {
        options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
    }
}