using App.Core.Interface.IServices;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Service
{

    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _http;
        private readonly string _apiKey = "AIzaSyDVlbUhNbRRTDB1VjAxcs8l5Dv5mZ_4Ras";

        public GeminiService(HttpClient http, IConfiguration config)
        {
            _http = http;
            //_apiKey = config["Gemini:ApiKey"] ?? Environment.GetEnvironmentVariable("GEMINI_API_KEY");
            //if (string.IsNullOrWhiteSpace(_apiKey))
            //    throw new InvalidOperationException("GEMINI_API_KEY not configured.");
        }

        public async Task<string> AskAsync(string prompt, string model = "gemini-2.5-flash")
        {
            var url = $"https://generativelanguage.googleapis.com/v1beta/models/{model}:generateContent";

            var payload = new
            {
                contents = new[]
                {
                    new
                    {
                        parts = new[]
                        {
                            new { text = prompt }
                        }
                    }
                },
                //generationConfig = new
                //{
                //    temperature = 0.3,
                //    maxOutputTokens = 100
                //}
            };



            using var req = new HttpRequestMessage(HttpMethod.Post, url);
            req.Headers.Add("x-goog-api-key", _apiKey);
            req.Content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            using var resp = await _http.SendAsync(req);
            resp.EnsureSuccessStatusCode();

            var json = await resp.Content.ReadAsStringAsync();

            // Parse response safely (the documented path: candidates[0].content.parts[0].text)
            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (root.TryGetProperty("candidates", out var candidates) &&
                candidates.GetArrayLength() > 0)
            {
                var candidate = candidates[0];
                if (candidate.TryGetProperty("content", out var content) &&
                    content.TryGetProperty("parts", out var parts) &&
                    parts.GetArrayLength() > 0)
                {
                    return parts[0].GetProperty("text").GetString() ?? string.Empty;
                }
            }

            return string.Empty;
        }
    }
}
