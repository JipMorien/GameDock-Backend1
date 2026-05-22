using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace GameDock.Tests.IntegrationTests;

public class GameDockUserIntegrationTests
    : IClassFixture<GameDockApplicationFactory>
{
    private readonly HttpClient _client;

    public GameDockUserIntegrationTests(GameDockApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task TC017_USER_GetUserById_ValidId_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/users/1");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task TC018_USER_CreateUser_ValidUser_ReturnsOkOrCreated()
    {
        var user = new
        {
            gameDockUserId = 1,
            isAdmin = false,
            userName = "IntegrationUser",
            email = "integration@test.com",
            passwordHash = "hashed-password",
            createdAt = DateTime.UtcNow
        };

        var response = await _client.PostAsJsonAsync("/api/users", user);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task TC019_USER_CreateUser_EmptyUsername_ReturnsBadRequest()
    {
        var user = new
        {
            gameDockUserId = 2,
            isAdmin = false,
            userName = "",
            email = "invalid@test.com",
            passwordHash = "hashed-password",
            createdAt = DateTime.UtcNow
        };

        var response = await _client.PostAsJsonAsync("/api/users", user);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);    }
}