using AnagramSolver.Contracts.Interfaces;
using AnagramSolver.Contracts.Models;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using OpenAI.Responses;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.Services
{
    public class AiChatService : IAiChatService
    {
        private readonly Kernel _kernel;
        private readonly IChatCompletionService _chatCompletionService;
        private readonly IInMemoryChatHistory _inMemoryChatHistory;

        public AiChatService(Kernel kernel, IChatCompletionService chatCompletionService, IInMemoryChatHistory inMemoryChatHistory)
        {
            _kernel = kernel;
            _chatCompletionService = chatCompletionService;
            _inMemoryChatHistory = inMemoryChatHistory;
        }

        public async Task<ChatResponse> GetResponseAsync(string sessionId, string prompt, CancellationToken ct)
        {
            var chatHistory = _inMemoryChatHistory.GetOrCreateHistory(sessionId);

            chatHistory.AddUserMessage(prompt);

            var executionSettings = new OpenAIPromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto()
            };

            var result = await _chatCompletionService.GetChatMessageContentAsync(
                chatHistory, 
                kernel: _kernel, 
                executionSettings: executionSettings);

            var responseMessage = result.Content ?? "Error. No answer from AI.";

            chatHistory.AddAssistantMessage(responseMessage);

            return new ChatResponse { Response = responseMessage, SessionId = sessionId} ;
        }
    }
}
