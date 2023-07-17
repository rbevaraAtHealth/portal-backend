using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace CodeMatcherV2Api.ApiRequestModels
{
    public class EmbedScheduledRunReqModel
    {
        [JsonProperty(PropertyName = "segment")]
        public string Segment { get; set; }
        [JsonProperty(PropertyName = "run_schedule")]
        public string  Runschedule { get; set; }
        [JsonProperty(PropertyName = "threshold")]
        public float Threshold { get; set; }
    }
}
