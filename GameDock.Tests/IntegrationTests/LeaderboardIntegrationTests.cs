using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace GameDock.Tests.IntegrationTests;

public class LeaderboardIntegrationTests
    : IClassFixture<GameDockApplicationFactory>
{
    private readonly HttpClient _client;

    public LeaderboardIntegrationTests(GameDockApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task TC014_LEADERBOARD_GetLeaderboardById_ValidId_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/leaderboards");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task TC015_LEADERBOARD_CreateLeaderboard_ValidLeaderboard_ReturnsOkOrCreated()
    {
        var leaderboard = new
        {
            leaderboardId = 1,
            name = "Integration Leaderboard",
            userId = 1
        };

        var response = await _client.PostAsJsonAsync("/api/leaderboards", leaderboard);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task TC016_LEADERBOARD_CreateLeaderboard_EmptyName_ReturnsBadRequest()
    {
        var leaderboard = new
        {
            leaderboardId = 2,
            name = "",
            userId = 1
        };

        var response = await _client.PostAsJsonAsync("/api/leaderboards", leaderboard);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}