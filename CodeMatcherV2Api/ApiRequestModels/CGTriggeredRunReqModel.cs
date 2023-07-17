using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace CodeMatcherV2Api.ApiRequestModels
{
    public class CGTriggeredRunReqModel
    {
        [JsonProperty(PropertyName = "segment")]
        public string Segment { get; set; }
        [JsonProperty(PropertyName = "threshold")]
        public float Threshold { get; set; }
        [JsonProperty(PropertyName = "latest_link")]
        public string LatestLink { get; set; }
        [JsonProperty(PropertyName = "client_Id")]
        public string Client_Id { get; set; }

    }
}
