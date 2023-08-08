using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace CodeMatcherV2Api.ApiRequestModels
{
    public class CgUploadCsvReqModel
    {
        [JsonProperty(PropertyName = "csv_input")]
        public string CsvInput { get; set; }
        [JsonProperty(PropertyName = "segment")]
        public string Segment { get; set; }
        [JsonProperty(PropertyName = "threshold")]
        public List<float> Threshold { get; set; }
    }
}
