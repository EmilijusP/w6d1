using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using Microsoft.AspNetCore.Mvc;

namespace AnagramSolver.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AnalysisController : ControllerBase
{
    private readonly IFrequencyAnalysisService _frequencyAnalysisService;

    public AnalysisController(IFrequencyAnalysisService frequencyAnalysisService)
    {
        _frequencyAnalysisService = frequencyAnalysisService;
    }

    [HttpPost("frequency")]
    [ProducesResponseType(typeof(FrequencyAnalysisResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<FrequencyAnalysisResponse>> AnalyzeFrequencyAsync(
        [FromBody] FrequencyAnalysisRequest request,
        CancellationToken cancellationToken = default)
    {
        try
        {
            var result = await _frequencyAnalysisService.AnalyzeAsync(request, cancellationToken);
            return Ok(result);
        }
        catch (ArgumentNullException ex)
        {
            return BadRequest(new { error = ex.Message });
        }
    }
}
