namespace AnagramSolver.Contracts.Models;

/// <summary>
/// Response model containing the results of word frequency analysis.
/// </summary>
public class FrequencyAnalysisResponse
{
    /// <summary>
    /// Gets or sets the total count of words processed (after filtering stop words).
    /// </summary>
    public int TotalWords { get; set; }

    /// <summary>
    /// Gets or sets the count of unique words found in the text.
    /// </summary>
    public int UniqueWords { get; set; }

    /// <summary>
    /// Gets or sets the top most frequent words, ordered by count descending, then alphabetically.
    /// </summary>
    /// <remarks>
    /// Limited to a maximum of 10 words by default (configurable via <see cref="FrequencyAnalysisOptions.TopWordsCount"/>).
    /// </remarks>
    public IReadOnlyList<WordFrequencyItem> TopWords { get; set; } = [];

    /// <summary>
    /// Gets or sets the longest word found in the text.
    /// </summary>
    /// <remarks>
    /// In case of multiple words with the same length, the lexicographically smallest one is returned.
    /// </remarks>
    public string LongestWord { get; set; } = string.Empty;
}
