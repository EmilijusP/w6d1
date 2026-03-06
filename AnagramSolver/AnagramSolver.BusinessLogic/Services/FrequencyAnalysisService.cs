using System.Globalization;
using System.Text.RegularExpressions;
using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using Microsoft.Extensions.Options;

namespace AnagramSolver.BusinessLogic.Services;

public partial class FrequencyAnalysisService : IFrequencyAnalysisService
{
    private readonly IStopWordsProvider _stopWordsProvider;
    private readonly FrequencyAnalysisOptions _options;

    public FrequencyAnalysisService(
        IStopWordsProvider stopWordsProvider,
        IOptions<FrequencyAnalysisOptions> options)
    {
        ArgumentNullException.ThrowIfNull(stopWordsProvider);
        ArgumentNullException.ThrowIfNull(options);
        ArgumentNullException.ThrowIfNull(options.Value);

        _stopWordsProvider = stopWordsProvider;
        _options = options.Value;
    }

    public Task<FrequencyAnalysisResponse> AnalyzeAsync(
        FrequencyAnalysisRequest request,
        CancellationToken cancellationToken = default)
    {
        cancellationToken.ThrowIfCancellationRequested();

        ValidateInput(request);

        var normalizedText = NormalizeText(request.Text);
        var tokens = Tokenize(normalizedText);
        var filteredTokens = FilterStopWords(tokens);

        if (filteredTokens.Count == 0)
        {
            throw new ArgumentException("Text contains no valid words after filtering.", nameof(request));
        }

        var response = Aggregate(filteredTokens);

        return Task.FromResult(response);
    }

    private void ValidateInput(FrequencyAnalysisRequest request)
    {
        if (request == null)
        {
            throw new ArgumentNullException(nameof(request));
        }

        if (string.IsNullOrWhiteSpace(request.Text))
        {
            throw new ArgumentException("Text cannot be empty or whitespace.", nameof(request));
        }

        if (request.Text.Length > _options.MaxTextLength)
        {
            throw new ArgumentException($"Text exceeds maximum allowed length of {_options.MaxTextLength} characters.", nameof(request));
        }
    }

    private static string NormalizeText(string text)
    {
        var trimmed = text.Trim();
        var normalized = WhitespaceRegex().Replace(trimmed, " ");
        return normalized.ToLowerInvariant();
    }

    private static List<string> Tokenize(string normalizedText)
    {
        var matches = WordRegex().Matches(normalizedText);
        var tokens = new List<string>();

        foreach (Match match in matches)
        {
            var word = match.Value;
            var trimmedWord = word.Trim('\'', '-');
            
            if (!string.IsNullOrEmpty(trimmedWord) && ContainsLetter(trimmedWord))
            {
                tokens.Add(trimmedWord);
            }
        }

        return tokens;
    }

    private static bool ContainsLetter(string word)
    {
        foreach (var c in word)
        {
            if (char.IsLetter(c))
            {
                return true;
            }
        }
        return false;
    }

    private List<string> FilterStopWords(List<string> tokens)
    {
        var stopWords = _stopWordsProvider.GetStopWords();
        return tokens
            .Where(token => !stopWords.Contains(token))
            .ToList();
    }

    private FrequencyAnalysisResponse Aggregate(List<string> tokens)
    {
        var frequencyMap = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);

        foreach (var token in tokens)
        {
            if (frequencyMap.TryGetValue(token, out var count))
            {
                frequencyMap[token] = count + 1;
            }
            else
            {
                frequencyMap[token] = 1;
            }
        }

        var topWords = frequencyMap
            .OrderByDescending(kvp => kvp.Value)
            .ThenBy(kvp => kvp.Key, StringComparer.OrdinalIgnoreCase)
            .Take(_options.TopWordsCount)
            .Select(kvp => new WordFrequencyItem
            {
                Word = kvp.Key,
                Count = kvp.Value
            })
            .ToList();

        var longestWord = frequencyMap.Keys
            .OrderByDescending(w => w.Length)
            .ThenBy(w => w, StringComparer.OrdinalIgnoreCase)
            .FirstOrDefault() ?? string.Empty;

        return new FrequencyAnalysisResponse
        {
            TotalWords = tokens.Count,
            UniqueWords = frequencyMap.Count,
            TopWords = topWords,
            LongestWord = longestWord
        };
    }

    [GeneratedRegex(@"\s+")]
    private static partial Regex WhitespaceRegex();

    [GeneratedRegex(@"[\p{L}]+(?:['-][\p{L}]+)*")]
    private static partial Regex WordRegex();
}
