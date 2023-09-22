using Newtonsoft.Json;

namespace CodeMatcherV2Api.ApiRequestModels
{
    public class WeeklyEmbedTriggeredRunReqModel
    {
         [JsonProperty(PropertyName = "segment")]
        public string Segment { get; set; }
        [JsonProperty(PropertyName = "latest_link")]
        public string LatestLink { get; set; }
        public string ConnectionString { get; set; }
    }
}
