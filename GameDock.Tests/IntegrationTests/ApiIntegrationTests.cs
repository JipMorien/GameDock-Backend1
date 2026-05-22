using System.Net;
using System.Net.Http;
using Xunit;

namespace GameDock.Tests.IntegrationTests;

public class ApiIntegrationTests
    : IClassFixture<GameDockApplicationFactory>
{
    private readonly HttpClient _client;

    public ApiIntegrationTests(GameDockApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_Invalid_Endpoint_Returns_NotFound()
    {
        var response = await _client.GetAsync("/does-not-exist");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }
}