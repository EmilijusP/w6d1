using System.ComponentModel;
using AnagramSolver.Mcp.Services;
using ModelContextProtocol.Server;

namespace AnagramSolver.Mcp.Tools;

[McpServerToolType]
public static class FindAnagramsTool
{
    [McpServerTool(Name = "find_anagrams"), Description("Finds anagrams for a given word by querying the AnagramSolver API.")]
    public static async Task<string> FindAnagrams(
        AnagramApiClient apiClient,
        [Description("The word to find anagrams for")] string word,
        CancellationToken ct)
    {
        if (string.IsNullOrWhiteSpace(word))
        {
            return "Error: word must not be empty.";
        }

        try
        {
            var anagrams = await apiClient.GetAnagramsAsync(word.Trim(), ct);

            return anagrams.Count > 0
                ? $"Found {anagrams.Count} anagram(s) for \"{word}\": {string.Join(", ", anagrams)}"
                : $"No anagrams found for \"{word}\".";
        }
        catch (InvalidOperationException ex) when (ex.Message.StartsWith("Bad request"))
        {
            return $"Validation error: {ex.Message}";
        }
        catch (HttpRequestException ex)
        {
            return $"API error: {ex.Message}";
        }
        catch (TaskCanceledException) when (!ct.IsCancellationRequested)
        {
            return "Error: the request to the AnagramSolver API timed out.";
        }
    }
}
