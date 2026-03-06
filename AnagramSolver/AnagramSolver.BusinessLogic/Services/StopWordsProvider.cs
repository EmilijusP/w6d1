using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using Microsoft.Extensions.Options;

namespace AnagramSolver.BusinessLogic.Services;

public class StopWordsProvider : IStopWordsProvider
{
    private readonly HashSet<string> _stopWords;

    public StopWordsProvider(IOptions<FrequencyAnalysisOptions> options)
    {
        ArgumentNullException.ThrowIfNull(options?.Value);

        _stopWords = options.Value.StopWords
            .Select(w => w.Trim().ToLowerInvariant())
            .Where(w => !string.IsNullOrWhiteSpace(w))
            .Distinct()
            .ToHashSet(StringComparer.OrdinalIgnoreCase);
    }

    public HashSet<string> GetStopWords() => _stopWords;
}
