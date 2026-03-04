using AnagramSolver.Contracts.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.AI;
using AnagramSolver.Contracts.Models;

namespace AnagramSolver.Api.Controllers
{
    [Route("api/ai/chat")]
    [ApiController]
    public class AiChatController : ControllerBase
    {
        private readonly IAiChatService _aiChatService;
        private readonly IInMemoryChatHistory _inMemoryChatHistory;

        public AiChatController(IAiChatService aiChatService, IInMemoryChatHistory inMemoryChatHistory)
        {
            _aiChatService = aiChatService;
            _inMemoryChatHistory = inMemoryChatHistory;
        }

        [HttpPost]
        public async Task<ActionResult<Contracts.Models.ChatResponse>> PostMessage([FromBody] Contracts.Models.ChatRequest request, CancellationToken ct = default)
        {
            if (string.IsNullOrWhiteSpace(request.Message))
            {
                return BadRequest("Message cannot be empty.");
            }

            if (string.IsNullOrWhiteSpace(request.SessionId))
            {
                return BadRequest("SessionId cannot be empty.");
            }

            var aiResponse = await _aiChatService.GetResponseAsync(request.SessionId, request.Message, ct);

            var response = new Contracts.Models.ChatResponse
            {
                Response = aiResponse.Response,
                SessionId = aiResponse.SessionId
            };

            return Ok(response);
        }

        [HttpGet("{sessionId}/history")]
        public IActionResult GetHistory(string sessionId)
        {
            if (string.IsNullOrWhiteSpace(sessionId))
            {
                return BadRequest("SessionId cannot be empty");
            }

            var history = _inMemoryChatHistory.GetHistory(sessionId);

            if (history == null)
            {
                return NotFound("Istorija nerasta.");
            }

            var formattedHistory = history.Select(h => new {
                Role = h.Role.ToString(),
                Content = h.Content
            });

            return Ok(formattedHistory);
        }
    }
}
