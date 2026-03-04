using AnagramSolver.Contracts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface IAiChatService
    {
        Task<ChatResponse> GetResponseAsync(string sessionId, string prompt, CancellationToken ct);
    }
}
