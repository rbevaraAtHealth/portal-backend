using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Collections;

namespace CodeMatcherV2Api.ApiRequestModels
{
    public class CgUploadCsvReqModel
    {
        [JsonProperty(PropertyName = "csv_input")]
        public string CsvInput { get; set; }
        [JsonProperty(PropertyName = "segment")]
        public string Segment { get; set; }
        [JsonProperty(PropertyName = "threshold")]
        public float Threshold { get; set; }
    }
}
