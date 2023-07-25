﻿using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;

namespace CodeMatcherV2Api.ApiResponeModel
{
    public class WeeklyEmbedTriggeredRunResModel
    {
        [JsonProperty(PropertyName = "message")]
        public string Message { get; set; }
        [JsonProperty(PropertyName = "reference")]
        public string Reference { get; set; }
    }
}
