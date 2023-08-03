using Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace CodeMatcherV2Api.Middlewares.HttpHelper
{
    public static class HttpHelper
    {
        
        public static async Task<HttpResponseMessage> Get_HttpClient(IHttpClientFactory httpClientFactory,string extensionurl)
        {
            var httpClient = httpClientFactory.CreateClient("CodeMatcher");
            var url = httpClient.BaseAddress + extensionurl;
            var response = await httpClient.GetAsync(url);
            //return await response.Content.ReadAsStringAsync();
            return response;
        }
        public static  async Task<HttpResponseMessage> Post_HttpClient(IHttpClientFactory httpClientFactory,object requestModel,string extensionurl)
        {
            var httpClient = httpClientFactory.CreateClient("CodeMatcher");
            var url = httpClient.BaseAddress + extensionurl;
            var requestContent = JsonConvert.SerializeObject(requestModel);
            var content = new StringContent(requestContent, System.Text.Encoding.UTF8, "application/json");
            var response = await httpClient.PostAsync(url,content);
            return response;
        }
        public static async Task<string> Put_HttpCliet(string url)
        {
            return "";
        }
        public static async Task<string> Delete_HttpCliet(string url)
        {
            return "";
        }
       
    }
   
}
