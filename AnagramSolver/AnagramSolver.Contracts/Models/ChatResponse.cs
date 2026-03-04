using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Contracts.Models
{
    public class ChatResponse
    {
        public string Response { get; set; } = string.Empty;

        public string SessionId { get; set; } = string.Empty;
    }
}
