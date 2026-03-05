using System.Net;
using System.Text.Json;
using AnagramSolver.Mcp.Services;
using AnagramSolver.Mcp.Tools;
using FluentAssertions;

namespace AnagramSolver.Mcp.Tests;

public class FindAnagramsToolTests
{
    private static AnagramApiClient CreateClientWithHandler(HttpMessageHandler handler)
    {
        var httpClient = new HttpClient(handler)
        {
            BaseAddress = new Uri("https://localhost:7079/")
        };
        return new AnagramApiClient(httpClient);
    }

    private static HttpMessageHandler CreateHandler(HttpStatusCode statusCode, string content)
    {
        return new FakeHttpMessageHandler(statusCode, content);
    }

    [Fact]
    public async Task FindAnagrams_ValidWord_ReturnsAnagramList()
    {
        // Arrange
        var anagrams = new[] { "listen", "silent", "enlist" };
        var handler = CreateHandler(HttpStatusCode.OK, JsonSerializer.Serialize(anagrams));
        var client = CreateClientWithHandler(handler);

        // Act
        var result = await FindAnagramsTool.FindAnagrams(client, "tinsel", CancellationToken.None);

        // Assert
        result.Should().Contain("listen");
        result.Should().Contain("silent");
        result.Should().Contain("enlist");
        result.Should().StartWith("Found 3 anagram(s)");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task FindAnagrams_EmptyOrNullWord_ReturnsValidationError(string? word)
    {
        // Arrange
        var handler = CreateHandler(HttpStatusCode.OK, "[]");
        var client = CreateClientWithHandler(handler);

        // Act
        var result = await FindAnagramsTool.FindAnagrams(client, word!, CancellationToken.None);

        // Assert
        result.Should().Be("Error: word must not be empty.");
    }

    [Fact]
    public async Task FindAnagrams_ApiBadRequest_ReturnsValidationError()
    {
        // Arrange
        var handler = CreateHandler(HttpStatusCode.BadRequest, "Bad input. Words should be made up of 3 or more letters.");
        var client = CreateClientWithHandler(handler);

        // Act
        var result = await FindAnagramsTool.FindAnagrams(client, "ab", CancellationToken.None);

        // Assert
        result.Should().StartWith("Validation error:");
        result.Should().Contain("Bad request");
    }

    [Fact]
    public async Task FindAnagrams_ApiTimeout_ReturnsTimeoutError()
    {
        // Arrange
        var handler = new TimeoutHttpMessageHandler();
        var client = CreateClientWithHandler(handler);

        // Act
        var result = await FindAnagramsTool.FindAnagrams(client, "tinsel", CancellationToken.None);

        // Assert
        result.Should().Be("Error: the request to the AnagramSolver API timed out.");
    }

    [Fact]
    public async Task FindAnagrams_NoAnagramsFound_ReturnsNoAnagramsMessage()
    {
        // Arrange
        var handler = CreateHandler(HttpStatusCode.OK, "[]");
        var client = CreateClientWithHandler(handler);

        // Act
        var result = await FindAnagramsTool.FindAnagrams(client, "xyz", CancellationToken.None);

        // Assert
        result.Should().Be("No anagrams found for \"xyz\".");
    }

    private class FakeHttpMessageHandler : HttpMessageHandler
    {
        private readonly HttpStatusCode _statusCode;
        private readonly string _content;

        public FakeHttpMessageHandler(HttpStatusCode statusCode, string content)
        {
            _statusCode = statusCode;
            _content = content;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            var response = new HttpResponseMessage(_statusCode)
            {
                Content = new StringContent(_content, System.Text.Encoding.UTF8, "application/json")
            };
            return Task.FromResult(response);
        }
    }

    private class TimeoutHttpMessageHandler : HttpMessageHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            throw new TaskCanceledException("The request was canceled due to the configured HttpClient.Timeout.",
                new TimeoutException(), CancellationToken.None);
        }
    }
}
