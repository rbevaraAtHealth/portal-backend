using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Text.Json.Serialization;

namespace CodeMatcherV2Api.ApiRequestModels
{
    public class CgScheduledRunReqModel
    {
        [JsonProperty(PropertyName = "segment")]
        public string Segment { get; set; }
        [JsonProperty(PropertyName = "run_schedule")]
        public string RunSchedule { get; set; }
        [JsonProperty(PropertyName = "threshold")]
        public string Threshold { get; set; }
        [JsonProperty(PropertyName = "latest_link")]
        public string LatestLink { get; set; }
        [JsonProperty(PropertyName = "client_Id")]
        public string ClientId { get; set; }
    }
}
