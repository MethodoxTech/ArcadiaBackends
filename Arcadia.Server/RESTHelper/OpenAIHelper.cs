using System.Net.Http.Headers;
using System.Text;

namespace Arcadia.Server.RESTHelper
{
    public static class OpenAIConfiguration
    {
        #region Configurations
        public const string Endpoint  = "https://api.openai.com/v1/chat/completions";
        public static string? APIToken { get; set; } = Environment.GetEnvironmentVariable("ARCADIA_OPENAI_KEY");
        public static bool IsAvailable => APIToken != null;
        public const int TokenSizeLimit = 4000;
        public const string Model = "gpt-4o-mini";
        #endregion
    }
    /// <summary>
    /// API for ChatGPT
    /// </summary>
    public static class OpenAIHelper
    {
        #region Methods
        public static string ChatGPTComplete(string system, string query)
        {
            using HttpClient httpClient = new()
            {
                BaseAddress = new Uri(OpenAIConfiguration.Endpoint)
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", OpenAIConfiguration.APIToken);

            string jsonRequestString = $$"""
                {
                    "messages": [
                        {
                            "role": "system",
                            "content": "{{JSONHelper.EscapeJsonString(system)}}"
                        },
                        {
                            "role": "user",
                            "content": "{{JSONHelper.EscapeJsonString(query)}}"
                        }
                    ],
                    "model": "{{OpenAIConfiguration.Model}}",
                    "max_tokens": {{OpenAIConfiguration.TokenSizeLimit}}
                }
                """;
            using StringContent jsonContent = new(jsonRequestString, Encoding.UTF8, "application/json");

            using HttpResponseMessage response = httpClient.PostAsync(string.Empty, jsonContent).Result;
            HttpResponseMessage message = response.EnsureSuccessStatusCode();

            string jsonResponse = response.Content.ReadAsStringAsync().Result;
            dynamic json = (IDictionary<string, object>)SimpleJson.SimpleJson.DeserializeObject(jsonResponse);
            string replyContent = json["choices"][0]["message"]["content"];
            return replyContent;
        }
        #endregion
    }

    public static class JSONHelper
    {
        /// <summary>
        /// Escapes a single JSON string literal
        /// </summary>
        public static string EscapeJsonString(string original, bool addQuotes = false)
        {
            string escaped = original
                .Replace("\r", string.Empty)
                .Replace("\\", "\\\\")
                .Replace("\"", "\\\"")
                .Replace("\n", "\\n");
            if (addQuotes)
                return $"\"{escaped}\"";
            else return escaped;
        }
    }
}
