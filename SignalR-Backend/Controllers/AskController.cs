using App.Core.Interface.IServices;
using Common.Dtos.GeminiDtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace SignalR_Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AskController : ControllerBase
    {
        private readonly IGeminiService _gemini;

        public AskController(IGeminiService gemini)
        {
            _gemini = gemini;
        }

        

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] AskRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.Question)) return BadRequest("question required");

            var answer = await _gemini.AskAsync(req.Question);
            return Ok(new { question = req.Question, answer });
        }
    }
}
