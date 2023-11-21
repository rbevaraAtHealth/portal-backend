using System.Net.Http.Headers;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CodeMatcher.Api.V2.Middlewares.CommonHelper
{
    public class ApiKeyHelper
    {
        public async Task<bool> ValidateApiKey(string apiKey)
        {
            if (apiKey == null || !IsValidApiKeyFormat(apiKey))
            {
                return false;
            }

            return await CheckApiKeyAuthorizationAsync(apiKey);
        }
        public static bool IsValidApiKeyFormat(string secretKey)
        {
            return Regex.IsMatch(secretKey, @"^sk-[a-zA-Z0-9]{32,}$");
        }
        public static async Task<bool> CheckApiKeyAuthorizationAsync(string apiKey)
        {
            using var httpClient = new HttpClient();

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var response = await httpClient.GetAsync("https://api.openai.com/v1/engines").ConfigureAwait(false);
            return response.IsSuccessStatusCode;
        }
    }
}
