using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace CodeMatcherV2Api.ApiRequestModels
{
    public class EmbedTriggeredRunReqModel
    {
        [JsonProperty(PropertyName = "segment")]
        public string Segment { get; set; }
        [JsonProperty(PropertyName = "threshold")]
        public float Threshold { get; set; }
    }
}
