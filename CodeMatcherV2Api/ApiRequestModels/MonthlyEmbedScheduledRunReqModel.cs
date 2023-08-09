using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace CodeMatcherV2Api.ApiRequestModels
{
    public class MonthlyEmbedScheduledRunReqModel
    {
        [JsonProperty(PropertyName = "segment")]
        public string Segment { get; set; }
        [JsonProperty(PropertyName = "run_schedule")]
        public string  Runschedule { get; set; }
        [JsonProperty(PropertyName = "threshold")]
        public string Threshold { get; set; }
    }
}
