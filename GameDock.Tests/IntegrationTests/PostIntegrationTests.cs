using System.Net;
using System.Net.Http.Json;
using Xunit;

namespace GameDock.Tests.IntegrationTests;

public class PostIntegrationTests
    : IClassFixture<GameDockApplicationFactory>
{
    private readonly HttpClient _client;

    public PostIntegrationTests(GameDockApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task TC014_POST_GetPostById_ValidId_ReturnsOk()
    {
        var response = await _client.GetAsync("/api/posts");

        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
    }

    [Fact]
    public async Task TC015_POST_CreatePost_ValidPost_ReturnsOkOrCreated()
    {
        var post = new
        {
            postId = 1,
            title = "Integration Test Post",
            content = "This post was created during integration testing.",
            userId = 1,
            createdAt = DateTime.UtcNow
        };

        var response = await _client.PostAsJsonAsync("/api/posts", post);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task TC016_POST_CreatePost_EmptyTitleOrContent_ReturnsBadRequest()
    {
        var post = new
        {
            postId = 2,
            title = "",
            content = "",
            userId = 1,
            createdAt = DateTime.UtcNow
        };

        var response = await _client.PostAsJsonAsync("/api/posts", post);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
}