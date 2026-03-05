using System.Net;
using System.Net.Http.Json;

namespace AnagramSolver.Mcp.Services;

public class AnagramApiClient
{
    private readonly HttpClient _httpClient;

    public AnagramApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<string>> GetAnagramsAsync(string word, CancellationToken ct)
    {
        var response = await _httpClient.GetAsync($"api/anagrams/{Uri.EscapeDataString(word)}", ct);

        if (response.StatusCode == HttpStatusCode.BadRequest)
        {
            var errorMessage = await response.Content.ReadAsStringAsync(ct);
            throw new InvalidOperationException($"Bad request: {errorMessage}");
        }

        response.EnsureSuccessStatusCode();

        var anagrams = await response.Content.ReadFromJsonAsync<List<string>>(ct);
        return anagrams ?? [];
    }
}
