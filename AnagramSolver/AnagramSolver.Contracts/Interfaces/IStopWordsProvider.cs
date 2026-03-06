namespace AnagramSolver.Contracts.Interfaces;

/// <summary>
/// Provider for retrieving the configured set of stop words to be excluded from frequency analysis.
/// </summary>
public interface IStopWordsProvider
{
    /// <summary>
    /// Gets the set of stop words that should be excluded from word frequency analysis.
    /// </summary>
    /// <returns>A case-insensitive HashSet containing normalized stop words.</returns>
    HashSet<string> GetStopWords();
}
