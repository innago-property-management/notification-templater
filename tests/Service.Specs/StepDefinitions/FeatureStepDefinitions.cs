namespace Service.Specs.StepDefinitions;

using System.Text;
using System.Text.Json;

using Bogus;

using FluentAssertions;

using Innago.Public.NotificationTemplater.API;
using Innago.Public.NotificationTemplater.API.HandlerModels;

using Microsoft.Extensions.Caching.Distributed;

using Moq;

using static Innago.Public.NotificationTemplater.API.Handlers;

using Reqnroll;

using Scriban;

[Binding]
public class FeatureStepDefinitions
{
    private Faker ValueGenerator { get; } = new();
    private string? template;
    private string? model;
    private string? result;
    private string? key;
    private IDistributedCache distributedCache = MakeCache();

    [Given("valid template")]
    public void GivenValidTemplate()
    {
        this.template = "Hello, {{ name }}";
    }

    [Given("valid model")]
    public void GivenValidModel()
    {
        string firstName = this.ValueGenerator.Person.FirstName;

        this.model = $$"""
                       {
                          "name": "{{firstName}}"
                       }
                       """;
    }

    [When("Generate is called using template and model")]
    public void WhenGenerateIsCalledUsingTemplateAndModel()
    {
        this.result = Generate(new GenerateInput(this.template!, this.model!));
    }

    [Then("the result should be correct")]
    public void ThenTheResultShouldBeCorrect()
    {
        Template t = Template.Parse(this.template);
        object data = JsonSerializer.Deserialize<JsonElement>(this.model!);
        string expected = t.Render(data, member => $"{char.ToLower(member.Name[0])}{member.Name[1..]}");

        this.result.Should().Be(expected);
    }

    [Given("key")]
    public void GivenKey()
    {
        this.key = string.Join('.', this.ValueGenerator.Random.Words(5));
    }

    [When("Save is called using key and model")]
    public async Task WhenSaveIsCalledUsingKeyAndModel()
    {
        TemplateSaveInput input = new(this.key!, this.model!);
        await Handlers.SaveTemplateAsync(input, this.distributedCache, CancellationToken.None);
    }

    [Then("model should be saved")]
    public void ThenModelShouldBeSaved()
    {
        Mock.Get(this.distributedCache)
            .Verify(cache =>
                cache.SetAsync(this.key!, Encoding.UTF8.GetBytes(this.model!), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()));
    }

    private static IDistributedCache MakeCache()
    {
        Dictionary<string, byte[]> data = [];
        var mock = new Mock<IDistributedCache>(MockBehavior.Strict);

        mock.Setup(
            cache => cache.GetAsync(
                It.IsAny<string>(),
                It.IsAny<CancellationToken>())
        ).ReturnsAsync(
            (string k, CancellationToken _) =>
                data.TryGetValue(k, out byte[]? b) ? b : []);

        mock.Setup(
            cache => cache.SetAsync(
                It.IsAny<string>(),
                It.IsAny<byte[]>(),
                It.IsAny<DistributedCacheEntryOptions>(),
                It.IsAny<CancellationToken>())
        ).Callback<string, byte[], DistributedCacheEntryOptions, CancellationToken>(
            (k, value, _, _) => { data[k] = value; }
        ).Returns(Task.CompletedTask);

        return mock.Object;
    }

    [Given("valid saved template")]
    public async Task GivenValidSavedTemplate()
    {
        this.GivenValidModel();
        this.GivenKey();
        await this.WhenSaveIsCalledUsingKeyAndModel();
    }

    [When("Generate is called using key and model")]
    public async Task WhenGenerateIsCalledUsingKeyAndModel()
    {
        this.result = await GenerateFromSavedTemplateAsync(new GenerateFromSavedTemplateInput(this.key!, this.model!),
            this.distributedCache,
            CancellationToken.None);
    }

    [Given("unmatched model")]
    public void GivenUnmatchedModel()
    {
        string firstName = this.ValueGenerator.Person.FirstName;

        this.model = $$"""
                       {
                          "firstName": "{{firstName}}"
                       }
                       """;
    }

    [Given("empty model")]
    public void GivenEmptyModel()
    {
        this.model = string.Empty;
    }

    [Then("the result should be empty string")]
    public void ThenTheResultShouldBeEmptyString()
    {
        this.result.Should().Be(string.Empty);
    }

    [Given("invalid saved template")]
    public void GivenInvalidSavedTemplate()
    {
        this.template = "Hello, { name }";
    }
}