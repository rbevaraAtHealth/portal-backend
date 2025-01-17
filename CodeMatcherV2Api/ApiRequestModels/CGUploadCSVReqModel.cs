﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;

namespace CodeMatcherV2Api.ApiRequestModels
{
    public class CgUploadCsvReqModel
    {
        [JsonProperty(PropertyName = "csv_location")]
        public string CsvInput { get; set; }
        [JsonProperty(PropertyName = "segment")]
        public string Segment { get; set; }
        [JsonProperty(PropertyName = "threshold")]
        public List<string> Threshold { get; set; }
        public string ApiKey { get; set; }
    }
}
