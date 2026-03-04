using AnagramSolver.Contracts.Interfaces;
using Microsoft.SemanticKernel.ChatCompletion;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.BusinessLogic.Services
{
    public class InMemoryChatHistory : IInMemoryChatHistory
    {
        private readonly ConcurrentDictionary<string, ChatHistory> _chatHistories = new();

        private const string SystemPrompt = "You are a smart AI assistant, that helps finding anagrams and answering questions.";

        public ChatHistory GetOrCreateHistory(string sessionId)
        {
            return _chatHistories.GetOrAdd(sessionId, _ => new ChatHistory(SystemPrompt));
        }

        public ChatHistory? GetHistory(string sessionId)
        {
            _chatHistories.TryGetValue(sessionId, out var history);
            return history;
        }
    }
}
