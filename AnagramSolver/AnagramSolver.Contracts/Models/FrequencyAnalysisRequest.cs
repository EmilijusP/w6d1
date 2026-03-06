using System.ComponentModel.DataAnnotations;

namespace AnagramSolver.Contracts.Models;

/// <summary>
/// Request model for word frequency analysis.
/// </summary>
public class FrequencyAnalysisRequest
{
    /// <summary>
    /// Gets or sets the text to analyze for word frequency.
    /// </summary>
    /// <remarks>
    /// The text is processed case-insensitively, with stop words filtered out.
    /// Special characters are ignored, and only Unicode letters are considered valid word characters.
    /// </remarks>
    [Required(AllowEmptyStrings = false, ErrorMessage = "Text is required.")]
    public string Text { get; set; } = string.Empty;
}
