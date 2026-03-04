using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace AnagramSolver.Contracts.Interfaces
{
    public interface IInMemoryChatHistory
    {
        ChatHistory GetOrCreateHistory(string sessionId);

        ChatHistory GetHistory(string sessionId);
    }
}
