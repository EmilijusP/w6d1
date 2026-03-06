namespace AnagramSolver.Contracts.Models;

/// <summary>
/// Configuration options for the frequency analysis service.
/// </summary>
public class FrequencyAnalysisOptions
{
    /// <summary>
    /// The configuration section name in appsettings.json.
    /// </summary>
    public const string SectionName = "FrequencyAnalysis";

    /// <summary>
    /// Gets or sets the maximum allowed text length in characters. Default is 100,000.
    /// </summary>
    public int MaxTextLength { get; set; } = 100_000;

    /// <summary>
    /// Gets or sets the maximum number of top words to return. Default is 10.
    /// </summary>
    public int TopWordsCount { get; set; } = 10;

    /// <summary>
    /// Gets or sets the list of stop words to exclude from analysis.
    /// </summary>
    /// <remarks>
    /// Stop words are normalized to lowercase and compared case-insensitively.
    /// </remarks>
    public List<string> StopWords { get; set; } = [];
}
