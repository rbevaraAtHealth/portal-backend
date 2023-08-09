using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace CodeMatcherV2Api.ApiRequestModels
{
    public class CgTriggeredRunReqModel
    {
        [JsonProperty(PropertyName = "segment")]
        public string Segment { get; set; }
        [JsonProperty(PropertyName = "threshold")]
        public string Threshold { get; set; }
        [JsonProperty(PropertyName = "latest_link")]
        public string LatestLink { get; set; }
        [JsonProperty(PropertyName = "client_Id")]
        public string ClientId { get; set; }

    }
}
