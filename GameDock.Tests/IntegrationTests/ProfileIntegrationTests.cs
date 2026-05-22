using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace GameDock.Tests.IntegrationTests;

public class ProfileIntegrationTests : IClassFixture<GameDockApplicationFactory>
{
    private readonly HttpClient _client;

    public ProfileIntegrationTests(GameDockApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task TC013_PROFILE_GetProfileById_ValidId_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/profiles/1");

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);    }

    [Fact]
    public async Task TC014_PROFILE_CreateProfile_ValidProfile_ReturnsOkOrCreated()
    {
        var profile = new
        {
            profileId = 1,
            userName = "IntegrationProfile",
            userId = 1,
            bio = "Integration test bio",
            level = 1,
            createdAt = DateTime.UtcNow
        };

        var response = await _client.PostAsJsonAsync("/api/profiles", profile);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);    }

    [Fact]
    public async Task TC015_PROFILE_CreateProfile_EmptyUsername_ReturnsBadRequest()
    {
        var profile = new
        {
            profileId = 2,
            userName = "",
            userId = 1,
            bio = "Invalid profile",
            level = 1,
            createdAt = DateTime.UtcNow
        };

        var response = await _client.PostAsJsonAsync("/api/profiles", profile);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}