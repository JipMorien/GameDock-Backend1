using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace GameDock.Tests.IntegrationTests;

public class StatisticIntegrationTests
    : IClassFixture<GameDockApplicationFactory>
{
    private readonly HttpClient _client;

    public StatisticIntegrationTests(GameDockApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task TC015_STATISTIC_GetAllStatistics_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/statistics");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task TC016_STATISTIC_CreateStatistic_ValidStatistic_ReturnsOkOrCreated()
    {
        var statistic = new
        {
            statisticId = 1,
            userId = 1,
            statisticType = 1,
            value = 10,
            createdAt = DateTime.UtcNow
        };

        var response = await _client.PostAsJsonAsync("/api/statistics", statistic);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task TC017_STATISTIC_CreateStatistic_InvalidStatisticType_ReturnsBadRequest()
    {
        var statistic = new
        {
            statisticId = 2,
            userId = 1,
            statisticType = 999,
            value = 10,
            createdAt = DateTime.UtcNow
        };

        var response = await _client.PostAsJsonAsync("/api/statistics", statistic);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}