using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace CodeMatcher.Api.V2.ApiResponseModel
{
    public class ApiResModel
    {
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        [JsonProperty(PropertyName = "reference")]
        public string Reference { get; set; }

        public string CsvOutputDirectoryPath { get; set; }
    }
}
