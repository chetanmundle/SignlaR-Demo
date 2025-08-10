using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Core.Interface.IServices
{
    public interface IGeminiService
    {
        Task<string> AskAsync(string prompt, string model = "gemini-2.5-flash");
    }
}
