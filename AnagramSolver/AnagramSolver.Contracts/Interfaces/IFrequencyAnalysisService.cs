namespace AnagramSolver.Contracts.Interfaces;

using AnagramSolver.Contracts.Models;

/// <summary>
/// Service for performing word frequency analysis on text input.
/// </summary>
public interface IFrequencyAnalysisService
{
    /// <summary>
    /// Analyzes the frequency of words in the provided text.
    /// </summary>
    /// <param name="request">The request containing the text to analyze.</param>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A response containing word frequency statistics including top words, counts, and longest word.</returns>
    /// <exception cref="ArgumentNullException">Thrown when <paramref name="request"/> is null.</exception>
    /// <exception cref="ArgumentException">Thrown when the text is empty, whitespace-only, exceeds max length, or contains no valid words.</exception>
    Task<FrequencyAnalysisResponse> AnalyzeAsync(FrequencyAnalysisRequest request, CancellationToken cancellationToken = default);
}
