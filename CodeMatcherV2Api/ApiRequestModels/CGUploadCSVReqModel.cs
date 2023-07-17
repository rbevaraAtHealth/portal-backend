using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace CodeMatcherV2Api.ApiRequestModels
{
    public class CGUploadCSVReqModel
    {
        [JsonProperty(PropertyName = "csv_input")]
        public string CSVInput { get; set; }
        [JsonProperty(PropertyName = "segment")]
        public string Segment { get; set;}
        [JsonProperty(PropertyName = "threshold")]
        public float Threshold { get; set; }
    }
}
