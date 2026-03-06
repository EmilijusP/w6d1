namespace AnagramSolver.Contracts.Models;

/// <summary>
/// Represents a word and its frequency count in the analyzed text.
/// </summary>
public class WordFrequencyItem
{
    /// <summary>
    /// Gets or sets the word (normalized to lowercase).
    /// </summary>
    public string Word { get; set; } = string.Empty;

    /// <summary>
    /// Gets or sets the number of times this word appears in the analyzed text.
    /// </summary>
    public int Count { get; set; }
}