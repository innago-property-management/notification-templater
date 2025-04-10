using Service;

AppDomain.CurrentDomain.SetData("REGEX_DEFAULT_MATCH_TIMEOUT", TimeSpan.FromSeconds(2));

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
builder.ConfigureServices();

WebApplication app = builder.Build();
app.ConfigureRouting();

await app.RunAsync();